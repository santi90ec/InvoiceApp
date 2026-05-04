using FluentAssertions;
using InvoiceApp.Infrastracture.Xml;
using Xunit;

namespace InvoiceApp.Tests;

public class InvoiceXmlReaderTests
{
    private readonly InvoiceXmlReader _reader = new();

    [Fact]
    public async Task ReadFromXmlAsync_ValidInvoice_ReturnsInvoice()
    {
        var invoice = await _reader.ReadFromXmlAsync(ValidInvoiceXml);

        invoice.Issuer.Ruc.Should().Be("1790012345001");
        invoice.Buyer.IdentificationNumber.Should().Be("0102030405");
        invoice.InvoiceDate.Should().Be(new DateTime(2026, 5, 4));
        invoice.FinancialSummary.GrandTotal.Should().Be(11.20m);
        invoice.FinancialSummary.TaxesSummary.Should().ContainSingle();
        invoice.Items.Should().ContainSingle();
        invoice.Items[0].Description.Should().Be("Servicio validado");
    }

    [Fact]
    public async Task ReadFromXmlAsync_AuthorizedInvoiceWrapper_ReturnsInvoice()
    {
        var wrappedXml = $"""
            <autorizacion>
              <estado>AUTORIZADO</estado>
              <comprobante><![CDATA[{ValidInvoiceXml}]]></comprobante>
            </autorizacion>
            """;

        var invoice = await _reader.ReadFromXmlAsync(wrappedXml);

        invoice.Issuer.Ruc.Should().Be("1790012345001");
        invoice.Items.Should().ContainSingle();
    }

    [Fact]
    public async Task ReadFromXmlAsync_DtdContent_ThrowsInvalidDataException()
    {
        const string xml = """
            <!DOCTYPE factura [
              <!ENTITY xxe SYSTEM "file:///etc/passwd">
            ]>
            <factura>&xxe;</factura>
            """;

        var act = () => _reader.ReadFromXmlAsync(xml);

        await act.Should().ThrowAsync<InvalidDataException>();
    }

    [Fact]
    public async Task ReadFromXmlAsync_MissingRequiredElement_ThrowsInvalidDataException()
    {
        const string xml = """
            <factura>
              <infoTributaria />
              <infoFactura />
              <detalles />
            </factura>
            """;

        var act = () => _reader.ReadFromXmlAsync(xml);

        await act.Should().ThrowAsync<InvalidDataException>();
    }

    private const string ValidInvoiceXml = """
        <factura>
          <infoTributaria>
            <razonSocial>Proveedor Demo</razonSocial>
            <nombreComercial>Proveedor Demo</nombreComercial>
            <ruc>1790012345001</ruc>
          </infoTributaria>
          <infoFactura>
            <fechaEmision>04/05/2026</fechaEmision>
            <razonSocialComprador>Comprador Demo</razonSocialComprador>
            <identificacionComprador>0102030405</identificacionComprador>
            <totalSinImpuestos>10.00</totalSinImpuestos>
            <totalDescuento>0.00</totalDescuento>
            <totalConImpuestos>
              <totalImpuesto>
                <codigo>2</codigo>
                <codigoPorcentaje>2</codigoPorcentaje>
                <baseImponible>10.00</baseImponible>
                <valor>1.20</valor>
              </totalImpuesto>
            </totalConImpuestos>
            <importeTotal>11.20</importeTotal>
          </infoFactura>
          <detalles>
            <detalle>
              <codigoPrincipal>SRV-001</codigoPrincipal>
              <descripcion>Servicio validado</descripcion>
              <cantidad>1.00</cantidad>
              <precioUnitario>10.00</precioUnitario>
              <descuento>0.00</descuento>
              <precioTotalSinImpuesto>10.00</precioTotalSinImpuesto>
              <impuestos>
                <impuesto>
                  <codigo>2</codigo>
                  <codigoPorcentaje>2</codigoPorcentaje>
                  <tarifa>12.00</tarifa>
                  <baseImponible>10.00</baseImponible>
                  <valor>1.20</valor>
                </impuesto>
              </impuestos>
            </detalle>
          </detalles>
        </factura>
        """;
}
