using FluentAssertions;
using InvoiceApp.Application.UseCase;
using InvoiceApp.Infrastracture.Classification;
using InvoiceApp.Infrastracture.Xml;
using Xunit;

namespace InvoiceApp.Tests;

public class ProcessInvoiceUseCaseTests
{
    [Fact]
    public async Task ProcessSingleInvoiceAsync_ValidXmlFile_ReturnsClassificationResult()
    {
        var tempFile = Path.Combine(Directory.GetCurrentDirectory(), $"{Guid.NewGuid():N}.xml");
        await File.WriteAllTextAsync(tempFile, ValidInvoiceXml);

        try
        {
            var sut = new ProcessInvoiceUseCase(new InvoiceXmlReader(), new BasicInvoiceClassifier());

            var result = await sut.ProcessSingleInvoiceAsync(tempFile);

            result.Category.Should().Be("VatRefundCandidate");
            result.IsVatRefundEligible.Should().BeTrue();
            result.IssuerLegalName.Should().Be("Proveedor Demo");
            result.BuyerLegalName.Should().Be("Comprador Demo");
            result.InvoiceDate.Should().Be(new DateTime(2026, 5, 4));
            result.GrandTotal.Should().Be(11.20m);
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
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
