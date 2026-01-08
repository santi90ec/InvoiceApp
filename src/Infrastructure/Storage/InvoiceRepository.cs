using InvoiceApp.Application.Interfaces;
using InvoiceApp.Models;

namespace InvoiceApp.Infrastructure.Storage;

public class InvoiceRepository : IInvoiceRepository
{
    public async Task<IReadOnlyList<Invoice>> GetAllInvoicesByPathAsync(string path, CancellationToken ct = default)
    {
        var xml = await File.ReadAllTextAsync(path, ct);
        return await ParseXmlInvoices(path, ct);
    }

    private async Task<IReadOnlyList<Invoice>> ParseXmlInvoices(string path, CancellationToken ct = default)
    {
        var xmlFiles = Directory.EnumerateFiles(path, "*.xml", SearchOption.TopDirectoryOnly).ToList();
        var invoices = new List<Invoice>(xmlFiles.Count);
        foreach (var file in xmlFiles)
        {
            ct.ThrowIfCancellationRequested();
            invoices.Add(await GetInvoiceByXmlPathAsync(file, ct));
        }
        return invoices;
    }

    public Task<Invoice> GetInvoiceByXmlPathAsync(string xmlPath, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}