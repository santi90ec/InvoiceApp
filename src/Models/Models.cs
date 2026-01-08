using System.Collections.ObjectModel;

namespace InvoiceApp.Models;

public class Invoice
{
    public Invoice(
        Issuer issuer,
        Buyer buyer,
        DateTime invoiceDate,
        FinancialSummary financialSummary,
        IEnumerable<InvoiceItem> items)
    {
        Issuer = issuer;
        Buyer = buyer;
        InvoiceDate = invoiceDate;
        FinancialSummary = financialSummary;
        Items = new ReadOnlyCollection<InvoiceItem>(items.ToList());
    }

    public Issuer Issuer { get; }
    public Buyer Buyer { get; }
    public DateTime InvoiceDate { get; }
    public FinancialSummary FinancialSummary { get; }
    public IReadOnlyList<InvoiceItem> Items { get; }
}

public class Buyer
{
    public Buyer(string legalName, string identificationNumber)
    {
        LegalName = legalName;
        IdentificationNumber = identificationNumber;
    }

    public string LegalName { get; }              // razonSocialComprador
    public string IdentificationNumber { get; }   // identificacionComprador
}

public class Issuer
{
    public Issuer(string legalName, string ruc, string tradeName)
    {
        LegalName = legalName;
        Ruc = ruc;
        TradeName = tradeName;
    }

    public string LegalName { get; }      // razonSocial
    public string Ruc { get; }            // ruc (13 dígitos)
    public string TradeName { get; }      // nombreComercial
}

public class FinancialSummary
{
    public FinancialSummary(
        decimal subtotalWithoutTaxes,
        decimal totalDiscount,
        decimal grandTotal,
        IEnumerable<TaxSummary> taxesSummary)
    {
        SubtotalWithoutTaxes = subtotalWithoutTaxes;
        TotalDiscount = totalDiscount;
        GrandTotal = grandTotal;
        TaxesSummary = new ReadOnlyCollection<TaxSummary>([.. taxesSummary]);
    }

    public decimal SubtotalWithoutTaxes { get; }      // totalSinImpuestos
    public decimal TotalDiscount { get; }             // totalDescuento
    public decimal GrandTotal { get; }                // importeTotal
    public IReadOnlyList<TaxSummary> TaxesSummary { get; } // totalConImpuestos
}

public class TaxSummary
{
    public TaxSummary(int taxCode, int rateCode, decimal baseAmount, decimal taxAmount)
    {
        TaxCode = taxCode;
        RateCode = rateCode;
        BaseAmount = baseAmount;
        TaxAmount = taxAmount;
    }

    public int TaxCode { get; }          // codigo (2=IVA, 3=ICE)
    public int RateCode { get; }         // codigoPorcentaje (2=12%, 6=0%, 7=No aplica)
    public decimal BaseAmount { get; }   // baseImponible
    public decimal TaxAmount { get; }
}

public class InvoiceItem
{
    public InvoiceItem(
            string itemCode,
            string description,
            decimal quantity,
            decimal unitPrice,
            decimal discount,
            decimal lineSubtotal,
            IEnumerable<ItemTax> taxes)
    {
        ItemCode = itemCode;
        Description = description;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
        LineSubtotal = lineSubtotal;
        Taxes = new ReadOnlyCollection<ItemTax>(taxes.ToList());
    }

    public string ItemCode { get; }              // codigoPrincipal
    public string Description { get; }           // descripcion (CRÍTICO para agente)
    public decimal Quantity { get; }             // cantidad
    public decimal UnitPrice { get; }            // precioUnitario
    public decimal Discount { get; }             // descuento
    public decimal LineSubtotal { get; }         // precioTotalSinImpuesto
    public IReadOnlyList<ItemTax> Taxes { get; } // impuestos


}

public class ItemTax
{
    public ItemTax(
        int taxCode,
        int rateCode,
        decimal rate,
        decimal baseAmount,
        decimal taxAmount)
    {
        TaxCode = taxCode;
        RateCode = rateCode;
        Rate = rate;
        BaseAmount = baseAmount;
        TaxAmount = taxAmount;
    }

    public int TaxCode { get; }          // codigo (2=IVA, 3=ICE)
    public int RateCode { get; }         // codigoPorcentaje
    public decimal Rate { get; }         // tarifa (ej: 12, 0)
    public decimal BaseAmount { get; }   // baseImponible
    public decimal TaxAmount { get; }    // valor
}


