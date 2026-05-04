namespace InvoiceApp.Presentation.Flow;

public static class InvoiceCategoryOptions
{
    public const string Unclassified = "Sin clasificar";

    public static readonly IReadOnlyList<string> Values =
    [
        "Medicinas",
        "Alimentos",
        "Servicios básicos",
        "Vestimenta",
        "Educación",
        "Otros",
        Unclassified
    ];

    public static bool IsKnown(string? category)
    {
        return !string.IsNullOrWhiteSpace(category) && Values.Contains(category);
    }
}
