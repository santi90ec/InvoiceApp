using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.UseCase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InvoiceApp.Presentation.Pages;

public class IndexModel : PageModel
{
    private readonly ProcessInvoiceUseCase _useCase;

    public IndexModel(ProcessInvoiceUseCase useCase)
    {
        _useCase = useCase;
    }

    [BindProperty] public IFormFile? XmlFile { get; set; }
    [BindProperty] public List<IFormFile> FolderFiles { get; set; } = new();

    public List<InvoiceClassificationResult> Results { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnPostProcessFileAsync(CancellationToken ct)
    {
        try
        {
            if (XmlFile is null || XmlFile.Length == 0)
            {
                ErrorMessage = "Selecciona un archivo XML.";
                return Page();
            }

            var tempDir = EnsureUploadsRoot();
            var tempFilePath = Path.Combine(tempDir, $"{Guid.NewGuid():N}.xml");

            await using (var fs = System.IO.File.Create(tempFilePath))
                await XmlFile.CopyToAsync(fs, ct);

            var result = await _useCase.ProcessSingleInvoiceAsyn(tempFilePath, ct);
            Results = [result];
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostProcessFolderAsync(CancellationToken ct)
    {
        try
        {
            if (FolderFiles is null || FolderFiles.Count == 0)
            {
                ErrorMessage = "Selecciona una carpeta (o múltiples XML).";
                return Page();
            }

            // Create a per-run folder to mimic "folder mode"
            var runFolder = Path.Combine(EnsureUploadsRoot(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(runFolder);

            foreach (var file in FolderFiles)
            {
                if (file.Length == 0) continue;

                // Preserve filename; folder structure may come in file.FileName in some browsers,
                // but for POC we just flatten to filename.
                var safeName = Path.GetFileName(file.FileName);
                if (!safeName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)) continue;

                var dest = Path.Combine(runFolder, safeName);

                await using var fs = System.IO.File.Create(dest);
                await file.CopyToAsync(fs, ct);
            }

            var results = await _useCase.ProcessMultipleInvoicesAsync(runFolder, ct);
            Results = [.. results];
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }

        return Page();
    }

    private static string EnsureUploadsRoot()
    {
        // local folder in the web app root: ./uploads
        var root = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        Directory.CreateDirectory(root);
        return root;
    }
}
