using InvoiceApp.Presentation.Flow;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InvoiceApp.Presentation.Pages;

public class Paso4Model : PageModel
{
    public List<InvoiceFlowItem> Invoices { get; private set; } = [];

    public void OnGet()
    {
        Invoices = InvoiceFlowSession.GetInvoices(HttpContext.Session);
    }
}
