using InvoiceApp.Application.Interfaces;
using InvoiceApp.Models;
using InvoiceApp.Infrastracture.Xml;

namespace InvoiceApp.Infrastracture.Storage;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly InvoiceXmlReader _reader = new();

    public async Task<IReadOnlyList<Invoice>> GetAllInvoicesByPathAsync(string path, CancellationToken ct = default)
    {
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
        return await _reader.ReadFromFileAsync(xmlPath, ct);
    }
}
