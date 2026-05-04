using InvoiceApp.Presentation.Flow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InvoiceApp.Presentation.Pages;

public class Paso3Model : PageModel
{
    [BindProperty] public List<InvoiceCategoryInput> InvoiceCategories { get; set; } = [];

    public IReadOnlyList<string> Categories { get; } = InvoiceCategoryOptions.Values;
    public List<InvoiceFlowItem> Invoices { get; private set; } = [];
    public string? ErrorMessage { get; private set; }

    public IActionResult OnGet()
    {
        Invoices = InvoiceFlowSession.GetInvoices(HttpContext.Session);
        return Page();
    }

    public IActionResult OnPost()
    {
        Invoices = InvoiceFlowSession.GetInvoices(HttpContext.Session);
        if (Invoices.Count == 0)
        {
            ErrorMessage = "Primero carga tus facturas para continuar.";
            return Page();
        }

        foreach (var invoice in Invoices)
        {
            var input = InvoiceCategories.FirstOrDefault(item => item.Id == invoice.Id);
            if (input is null || !InvoiceCategoryOptions.IsKnown(input.Category))
            {
                invoice.ManualCategory = InvoiceCategoryOptions.Unclassified;
                continue;
            }

            invoice.ManualCategory = input.Category;
        }

        InvoiceFlowSession.SaveInvoices(HttpContext.Session, Invoices);

        if (Invoices.Any(invoice => invoice.ManualCategory == InvoiceCategoryOptions.Unclassified))
        {
            ErrorMessage = "Por favor clasifica todas las facturas para continuar";
            return Page();
        }

        return RedirectToPage("/Paso4");
    }
}

public sealed class InvoiceCategoryInput
{
    public string Id { get; set; } = string.Empty;
    public string Category { get; set; } = InvoiceCategoryOptions.Unclassified;
}
