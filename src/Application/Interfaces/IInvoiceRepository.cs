using InvoiceApp.Models;

namespace InvoiceApp.Application.Interfaces;

public interface IInvoiceRepository
{
    public Task<Invoice> GetInvoiceByXmlContent(string xmlPath, CancellationToken ct = default);
    public Task<IReadOnlyList<Invoice>> GetAllInvoicesbyPath(string xmlPath, CancellationToken ct = default);

}