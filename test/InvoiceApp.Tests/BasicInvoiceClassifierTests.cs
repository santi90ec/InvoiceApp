using FluentAssertions;
using InvoiceApp.Infrastracture.Classification;
using InvoiceApp.Models;
using Xunit;

namespace InvoiceApp.Tests;

public class BasicInvoiceClassifierTests
{
    [Fact]
    public async Task ClassifyInvoiceAsync_InvoiceWithVat_ReturnsEligibleResult()
    {
        var invoice = new Invoice(
            new Issuer("Proveedor Demo", "1790012345001", "Proveedor Demo"),
            new Buyer("Comprador Demo", "0102030405"),
            new DateTime(2026, 5, 4),
            new FinancialSummary(10m, 0m, 11.2m, [new TaxSummary(2, 2, 10m, 1.2m)]),
            [new InvoiceItem("SRV-001", "Servicio validado", 1m, 10m, 0m, 10m, [])]);

        var classifier = new BasicInvoiceClassifier();

        var result = await classifier.ClassifyInvoiceAsync(invoice);

        result.InvoiceNumber.Should().Be("20260504");
        result.Category.Should().Be("VatRefundCandidate");
        result.IsVatRefundEligible.Should().BeTrue();
        result.Confidence.Should().Be(1m);
        result.IssuerLegalName.Should().Be("Proveedor Demo");
        result.IssuerRuc.Should().Be("1790012345001");
        result.BuyerLegalName.Should().Be("Comprador Demo");
        result.InvoiceDate.Should().Be(new DateTime(2026, 5, 4));
        result.GrandTotal.Should().Be(11.2m);
    }
}
