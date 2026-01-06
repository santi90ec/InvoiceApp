using System.Threading;
using System.Threading.Tasks;
using InvoiceApp.Models;

namespace InvoiceApp.Application.Interfaces
{
    public interface IInvoiceReader
    {
        Task<Invoice> ReadFromFileAsync(string filePath, CancellationToken ct = default);

        Task<Invoice> ReadFromXmlAsync(string xmlContent, CancellationToken ct = default);
        Task<List<Invoice>> ReadFromXmlMultipleAsync(string filePath, CancellationToken ct = default);
    }
}