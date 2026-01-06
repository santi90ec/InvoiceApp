using InvoiceApp.Application.DTOs;
using InvoiceApp.Models;

namespace InvoiceApp.Application.Interfaces
{
    public interface IInvoiceClassifier
    {
        Task<InvoiceClassificationResult> ClassifyInvoiceAsync(Invoice invoice, CancellationToken ct = default);
    }
}