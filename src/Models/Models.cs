using System.Collections.ObjectModel;

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
public class InvoiceItem
{
    public InvoiceItem(string description, decimal lineBaseAmount, VatTax? vat)
    {
        Description = description;
        LineBaseAmount = lineBaseAmount;
        Vat = vat;
    }

    public string Description { get; }

    // Subtotal / base sin impuestos (clave para reglas)
    public decimal LineBaseAmount { get; }

    // null = sin IVA o no informado
    public VatTax? Vat { get; }
}
public class VatTax
{
    public VatTax(decimal rate, decimal baseAmount, decimal amount)
    {
        Rate = rate;
        BaseAmount = baseAmount;
        Amount = amount;
    }

    public decimal Rate { get; }
    public decimal BaseAmount { get; }
    public decimal Amount { get; }
}