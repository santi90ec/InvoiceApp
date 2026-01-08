using InvoiceApp.Models;

namespace InvoiceApp.Application.Interfaces;

public interface IInvoiceRepository
{
    public Task<Invoice> GetInvoiceByXmlPathAsync(string xmlPath, CancellationToken ct = default);
    public Task<IReadOnlyList<Invoice>> GetAllInvoicesByPathAsync(string path, CancellationToken ct = default);

}