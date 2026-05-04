using InvoiceApp.Presentation.Flow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InvoiceApp.Presentation.Pages;

public class Paso2Model : PageModel
{
    public List<InvoiceFlowItem> Invoices { get; private set; } = [];

    public IActionResult OnGet()
    {
        Invoices = InvoiceFlowSession.GetInvoices(HttpContext.Session);
        return Page();
    }
}
