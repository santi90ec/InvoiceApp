namespace InvoiceApp.Application.DTOs;

public class InvoiceClassificationResult(
    string invoiceNumber,
    string category,
    bool isVatRefundEligible,
    decimal confidence,
    string? reason = null)
{
    public string InvoiceNumber { get; } = invoiceNumber;
    public string Category { get; } = category;
    public bool IsVatRefundEligible { get; } = isVatRefundEligible;
    public decimal Confidence { get; } = confidence;
    public string? Reason { get; } = reason;
}