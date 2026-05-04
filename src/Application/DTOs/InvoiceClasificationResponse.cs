namespace InvoiceApp.Application.DTOs;

public class InvoiceClassificationResult(
    string invoiceNumber,
    string category,
    bool isVatRefundEligible,
    decimal confidence,
    string? reason = null,
    string? issuerLegalName = null,
    string? buyerLegalName = null,
    DateTime? invoiceDate = null,
    decimal? grandTotal = null)
{
    public string InvoiceNumber { get; } = invoiceNumber;
    public string Category { get; } = category;
    public bool IsVatRefundEligible { get; } = isVatRefundEligible;
    public decimal Confidence { get; } = confidence;
    public string? Reason { get; } = reason;
    public string? IssuerLegalName { get; } = issuerLegalName;
    public string? BuyerLegalName { get; } = buyerLegalName;
    public DateTime? InvoiceDate { get; } = invoiceDate;
    public decimal? GrandTotal { get; } = grandTotal;
}
