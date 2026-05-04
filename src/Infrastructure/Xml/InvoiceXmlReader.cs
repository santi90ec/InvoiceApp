using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using InvoiceApp.Application.Interfaces;
using InvoiceApp.Models;

namespace InvoiceApp.Infrastracture.Xml;

public class InvoiceXmlReader : IInvoiceReader
{
    private const long MaxXmlBytes = 1_000_000;

    public async Task<Invoice> ReadFromFileAsync(string filePath, CancellationToken ct = default)
    {
        ValidateXmlFilePath(filePath);

        var fileInfo = new FileInfo(filePath);
        if (fileInfo.Length == 0)
        {
            throw new InvalidDataException("XML file is empty.");
        }

        if (fileInfo.Length > MaxXmlBytes)
        {
            throw new InvalidDataException("XML file exceeds the maximum allowed size.");
        }

        var xmlContent = await File.ReadAllTextAsync(filePath, ct);
        return await ReadFromXmlAsync(xmlContent, ct);
    }

    public Task<Invoice> ReadFromXmlAsync(string xmlContent, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(xmlContent))
        {
            throw new InvalidDataException("XML content is empty.");
        }

        if (xmlContent.Length > MaxXmlBytes)
        {
            throw new InvalidDataException("XML content exceeds the maximum allowed size.");
        }

        var document = LoadSafeXml(xmlContent);
        var invoiceDocument = UnwrapAuthorizedInvoice(document);
        var invoice = ParseInvoice(invoiceDocument);

        return Task.FromResult(invoice);
    }

    public async Task<List<Invoice>> ReadFromXmlMultipleAsync(string filePath, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("Path is required.", nameof(filePath));
        }

        var root = Path.GetFullPath(filePath);
        if (!Directory.Exists(root))
        {
            throw new DirectoryNotFoundException("XML folder was not found.");
        }

        var invoices = new List<Invoice>();
        foreach (var xmlFile in Directory.EnumerateFiles(root, "*.xml", SearchOption.TopDirectoryOnly))
        {
            ct.ThrowIfCancellationRequested();
            invoices.Add(await ReadFromFileAsync(xmlFile, ct));
        }

        return invoices;
    }

    private static void ValidateXmlFilePath(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path is required.", nameof(filePath));
        }

        if (!string.Equals(Path.GetExtension(filePath), ".xml", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidDataException("Only XML files are allowed.");
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("XML file was not found.");
        }
    }

    private static XDocument LoadSafeXml(string xmlContent)
    {
        var settings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Prohibit,
            XmlResolver = null,
            MaxCharactersInDocument = MaxXmlBytes
        };

        try
        {
            using var stringReader = new StringReader(xmlContent);
            using var xmlReader = XmlReader.Create(stringReader, settings);
            return XDocument.Load(xmlReader);
        }
        catch (XmlException ex)
        {
            throw new InvalidDataException("XML content is malformed or unsafe.", ex);
        }
    }

    private static XDocument UnwrapAuthorizedInvoice(XDocument document)
    {
        if (document.Root is null)
        {
            throw new InvalidDataException("XML document does not have a root element.");
        }

        if (!IsElement(document.Root, "autorizacion"))
        {
            return document;
        }

        var comprobante = Child(document.Root, "comprobante")?.Value;
        if (string.IsNullOrWhiteSpace(comprobante))
        {
            throw new InvalidDataException("Authorized XML does not contain invoice content.");
        }

        return LoadSafeXml(comprobante);
    }

    private static Invoice ParseInvoice(XDocument document)
    {
        var root = document.Root;
        if (root is null || !IsElement(root, "factura"))
        {
            throw new InvalidDataException("XML document is not an invoice.");
        }

        var infoTributaria = RequiredChild(root, "infoTributaria");
        var infoFactura = RequiredChild(root, "infoFactura");

        var issuer = new Issuer(
            RequiredValue(infoTributaria, "razonSocial"),
            RequiredValue(infoTributaria, "ruc"),
            OptionalValue(infoTributaria, "nombreComercial") ?? RequiredValue(infoTributaria, "razonSocial"));

        var buyer = new Buyer(
            RequiredValue(infoFactura, "razonSocialComprador"),
            RequiredValue(infoFactura, "identificacionComprador"));

        var financialSummary = new FinancialSummary(
            RequiredDecimal(infoFactura, "totalSinImpuestos"),
            OptionalDecimal(infoFactura, "totalDescuento"),
            RequiredDecimal(infoFactura, "importeTotal"),
            ParseTaxSummary(infoFactura));

        var items = ParseItems(root);

        return new Invoice(
            issuer,
            buyer,
            RequiredDate(infoFactura, "fechaEmision"),
            financialSummary,
            items);
    }

    private static IReadOnlyList<TaxSummary> ParseTaxSummary(XElement infoFactura)
    {
        var totalConImpuestos = Child(infoFactura, "totalConImpuestos");
        if (totalConImpuestos is null)
        {
            return [];
        }

        return totalConImpuestos
            .Elements()
            .Where(element => IsElement(element, "totalImpuesto"))
            .Select(tax => new TaxSummary(
                RequiredInt(tax, "codigo"),
                RequiredInt(tax, "codigoPorcentaje"),
                RequiredDecimal(tax, "baseImponible"),
                RequiredDecimal(tax, "valor")))
            .ToList();
    }

    private static IReadOnlyList<InvoiceItem> ParseItems(XElement root)
    {
        var detalles = RequiredChild(root, "detalles");
        var items = detalles
            .Elements()
            .Where(element => IsElement(element, "detalle"))
            .Select(detalle => new InvoiceItem(
                RequiredValue(detalle, "codigoPrincipal"),
                RequiredValue(detalle, "descripcion"),
                RequiredDecimal(detalle, "cantidad"),
                RequiredDecimal(detalle, "precioUnitario"),
                OptionalDecimal(detalle, "descuento"),
                RequiredDecimal(detalle, "precioTotalSinImpuesto"),
                ParseItemTaxes(detalle)))
            .ToList();

        if (items.Count == 0)
        {
            throw new InvalidDataException("Invoice does not contain detail items.");
        }

        return items;
    }

    private static IReadOnlyList<ItemTax> ParseItemTaxes(XElement detalle)
    {
        var impuestos = Child(detalle, "impuestos");
        if (impuestos is null)
        {
            return [];
        }

        return impuestos
            .Elements()
            .Where(element => IsElement(element, "impuesto"))
            .Select(tax => new ItemTax(
                RequiredInt(tax, "codigo"),
                RequiredInt(tax, "codigoPorcentaje"),
                RequiredDecimal(tax, "tarifa"),
                RequiredDecimal(tax, "baseImponible"),
                RequiredDecimal(tax, "valor")))
            .ToList();
    }

    private static XElement RequiredChild(XElement parent, string localName)
    {
        return Child(parent, localName)
            ?? throw new InvalidDataException($"Required XML element '{localName}' was not found.");
    }

    private static XElement? Child(XElement parent, string localName)
    {
        return parent.Elements().FirstOrDefault(element => IsElement(element, localName));
    }

    private static bool IsElement(XElement element, string localName)
    {
        return string.Equals(element.Name.LocalName, localName, StringComparison.Ordinal);
    }

    private static string RequiredValue(XElement parent, string localName)
    {
        var value = OptionalValue(parent, localName);
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidDataException($"Required XML value '{localName}' was not found.");
        }

        return value;
    }

    private static string? OptionalValue(XElement parent, string localName)
    {
        return Child(parent, localName)?.Value.Trim();
    }

    private static decimal RequiredDecimal(XElement parent, string localName)
    {
        var value = RequiredValue(parent, localName);
        if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
        {
            throw new InvalidDataException($"XML value '{localName}' is not a valid decimal.");
        }

        return result;
    }

    private static decimal OptionalDecimal(XElement parent, string localName)
    {
        var value = OptionalValue(parent, localName);
        if (string.IsNullOrWhiteSpace(value))
        {
            return 0m;
        }

        if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
        {
            throw new InvalidDataException($"XML value '{localName}' is not a valid decimal.");
        }

        return result;
    }

    private static int RequiredInt(XElement parent, string localName)
    {
        var value = RequiredValue(parent, localName);
        if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
        {
            throw new InvalidDataException($"XML value '{localName}' is not a valid integer.");
        }

        return result;
    }

    private static DateTime RequiredDate(XElement parent, string localName)
    {
        var value = RequiredValue(parent, localName);
        var formats = new[] { "dd/MM/yyyy", "yyyy-MM-dd" };
        if (!DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
        {
            throw new InvalidDataException($"XML value '{localName}' is not a valid date.");
        }

        return result;
    }
}
