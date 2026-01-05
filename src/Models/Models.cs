namespace InvoiceApp.Models;

public class Invoice
{
    public Invoice(string invoiceNumber, IEnumerable<InvoiceItem> items)
    {
        InvoiceNumber = invoiceNumber;
        Items = new ReadOnlyCollection<InvoiceItem>(items.ToList());
    }
    public string InvoiceNumber { get; }
    public IReadOnlyList<InvoiceItem> Items { get; }
}
