using InvoiceApp.Application.DTOs;

namespace InvoiceApp.Presentation.Flow;

public sealed class InvoiceFlowItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string? InvoiceNumber { get; set; }
    public string? IssuerLegalName { get; set; }
    public string? IssuerRuc { get; set; }
    public string? BuyerLegalName { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public decimal? GrandTotal { get; set; }
    public bool IsVatRefundEligible { get; set; }
    public string ManualCategory { get; set; } = InvoiceCategoryOptions.Unclassified;

    public static InvoiceFlowItem FromResult(InvoiceClassificationResult result)
    {
        return new InvoiceFlowItem
        {
            InvoiceNumber = result.InvoiceNumber,
            IssuerLegalName = result.IssuerLegalName,
            IssuerRuc = result.IssuerRuc,
            BuyerLegalName = result.BuyerLegalName,
            InvoiceDate = result.InvoiceDate,
            GrandTotal = result.GrandTotal,
            IsVatRefundEligible = result.IsVatRefundEligible
        };
    }
}
