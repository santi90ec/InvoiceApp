using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Interfaces;
using InvoiceApp.Models;

namespace InvoiceApp.Infrastracture.Classification;

public class BasicInvoiceClassifier : IInvoiceClassifier
{
    public Task<InvoiceClassificationResult> ClassifyInvoiceAsync(Invoice invoice, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(invoice);
        ct.ThrowIfCancellationRequested();

        var hasVat = invoice.FinancialSummary.TaxesSummary.Any(tax => tax.TaxCode == 2 && tax.TaxAmount > 0);
        var category = hasVat ? "VatRefundCandidate" : "NoVatDetected";
        var reason = hasVat
            ? "Invoice contains IVA tax lines."
            : "Invoice does not contain positive IVA tax lines.";

        var result = new InvoiceClassificationResult(
            invoice.InvoiceDate.ToString("yyyyMMdd"),
            category,
            hasVat,
            1m,
            reason,
            invoice.Issuer.LegalName,
            invoice.Issuer.Ruc,
            invoice.Buyer.LegalName,
            invoice.InvoiceDate,
            invoice.FinancialSummary.GrandTotal);

        return Task.FromResult(result);
    }
}
