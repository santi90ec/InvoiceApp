using System.Xml.Linq;
using InvoiceApp.Application.Interfaces;
using InvoiceApp.Models;

namespace InvoiceApp.Infrastracture.Storage;

public class InvoiceRepository : IInvoiceRepository
{
    public async Task<IReadOnlyList<Invoice>> GetAllInvoicesByPathAsync(string path, CancellationToken ct = default)
    {
        var xml = await File.ReadAllTextAsync(path, ct);
        return await ParseXmlInvoices(path, ct);
    }

    private async Task<IReadOnlyList<Invoice>> ParseXmlInvoices(string path, CancellationToken ct = default)
    {
        var xmlFiles = Directory.EnumerateFiles(path, "*.xml", SearchOption.TopDirectoryOnly).ToList();
        var invoices = new List<Invoice>(xmlFiles.Count);
        foreach (var xmlfile in xmlFiles)
        {
            ct.ThrowIfCancellationRequested();
            var invoice = await GetInvoiceByXmlPathAsync(xmlfile, ct);
            invoices.Add(invoice);
        }
        return invoices.AsReadOnly();
    }

    public async Task<Invoice> GetInvoiceByXmlPathAsync(string xmlPath, CancellationToken ct = default)
    {
        // Leer contenido del archivo
        var xmlContent = await File.ReadAllTextAsync(xmlPath, ct);

        // Parsear y retornar
        return await ParseInvoiceFromXml(xmlContent);
    }

    private static Task<Invoice> ParseInvoiceFromXml(string xmlContent)
    {
        var doc = XDocument.Parse(xmlContent);
        // TODO: Parse 'doc' to create and return an Invoice instance
        throw new NotImplementedException("Parsing logic not implemented.");
    }
}