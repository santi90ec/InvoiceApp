using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Interfaces;
using InvoiceApp.Models;

namespace InvoiceApp.Application.UseCase;

public class ProcessInvoiceUseCase(IInvoiceReader reader, IInvoiceClassifier classifier)
{
    private readonly IInvoiceReader _reader = reader;
    private readonly IInvoiceClassifier _classifier = classifier;

    public async Task<InvoiceClassificationResult> ProcessSingleInvoiceAsyn(string filePath, CancellationToken ct = default)
    {
        Invoice invoice = await _reader.ReadFromXmlAsync(filePath, ct);
        InvoiceClassificationResult classificationResult = await _classifier.ClassifyInvoiceAsync(invoice, ct);
        return classificationResult;
    }

    public async Task<IReadOnlyList<InvoiceClassificationResult>> ProcessMultipleInvoicesAsync(string filePath, CancellationToken ct = default)
    {
        List<Invoice> invoices = await _reader.ReadFromXmlMultipleAsync(filePath, ct);
        List<InvoiceClassificationResult> results = [];
        foreach (var invoice in invoices)
        {
            ct.ThrowIfCancellationRequested();
            var classificationResult = await _classifier.ClassifyInvoiceAsync(invoice, ct);
            results.Add(classificationResult);
        }

        return [.. results];
    }

}