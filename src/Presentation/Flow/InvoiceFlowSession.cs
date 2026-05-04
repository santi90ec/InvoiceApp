using System.Text.Json;

namespace InvoiceApp.Presentation.Flow;

public static class InvoiceFlowSession
{
    private const string InvoicesKey = "InvoiceFlow.Invoices";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public static List<InvoiceFlowItem> GetInvoices(ISession session)
    {
        var json = session.GetString(InvoicesKey);
        if (string.IsNullOrWhiteSpace(json))
        {
            return [];
        }

        return JsonSerializer.Deserialize<List<InvoiceFlowItem>>(json, JsonOptions) ?? [];
    }

    public static void SaveInvoices(ISession session, IEnumerable<InvoiceFlowItem> invoices)
    {
        var json = JsonSerializer.Serialize(invoices.ToList(), JsonOptions);
        session.SetString(InvoicesKey, json);
    }
}
