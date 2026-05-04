using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.UseCase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InvoiceApp.Presentation.Pages;

public class Paso1Model(ProcessInvoiceUseCase useCase, ILogger<Paso1Model> logger) : PageModel
{
    private const long MaxXmlBytes = 1_000_000;
    private readonly ProcessInvoiceUseCase _useCase = useCase;
    private readonly ILogger<Paso1Model> _logger = logger;

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

            if (!IsValidXmlUpload(XmlFile, out var validationError))
            {
                ErrorMessage = validationError;
                return Page();
            }

            var tempDir = EnsureUploadsRoot();
            var tempFilePath = Path.Combine(tempDir, $"{Guid.NewGuid():N}.xml");

            await using (var fs = new FileStream(tempFilePath, FileMode.CreateNew, FileAccess.Write))
            {
                await XmlFile.CopyToAsync(fs, ct);
            }

            var result = await _useCase.ProcessSingleInvoiceAsync(tempFilePath, ct);
            Results = [result];
        }
        catch (InvalidDataException ex)
        {
            _logger.LogWarning(ex, "Uploaded XML file failed validation");
            ErrorMessage = "El XML no tiene el formato esperado o no es seguro.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error processing uploaded XML file");
            ErrorMessage = "No se pudo procesar el XML.";
        }

        return Page();
    }

    public async Task<IActionResult> OnPostProcessFolderAsync(CancellationToken ct)
    {
        try
        {
            if (FolderFiles is null || FolderFiles.Count == 0)
            {
                ErrorMessage = "Selecciona una carpeta o varios XML.";
                return Page();
            }

            var runFolder = Path.Combine(EnsureUploadsRoot(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(runFolder);

            var acceptedFiles = 0;
            foreach (var file in FolderFiles)
            {
                if (file.Length == 0) continue;
                if (!IsValidXmlUpload(file, out _)) continue;

                var safeName = Path.GetFileName(file.FileName);
                var dest = Path.Combine(runFolder, $"{Guid.NewGuid():N}_{safeName}");

                await using var fs = new FileStream(dest, FileMode.CreateNew, FileAccess.Write);
                await file.CopyToAsync(fs, ct);
                acceptedFiles++;
            }

            if (acceptedFiles == 0)
            {
                ErrorMessage = "Selecciona al menos un archivo XML válido.";
                return Page();
            }

            var results = await _useCase.ProcessMultipleInvoicesAsync(runFolder, ct);
            Results = [.. results];
        }
        catch (InvalidDataException ex)
        {
            _logger.LogWarning(ex, "Uploaded XML folder failed validation");
            ErrorMessage = "Uno o más XML no tienen el formato esperado o no son seguros.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error processing uploaded XML folder");
            ErrorMessage = "No se pudo procesar la carpeta XML.";
        }

        return Page();
    }

    private static bool IsValidXmlUpload(IFormFile file, out string errorMessage)
    {
        errorMessage = string.Empty;

        var safeName = Path.GetFileName(file.FileName);
        if (string.IsNullOrWhiteSpace(safeName) ||
            !safeName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
        {
            errorMessage = "Solo se permiten archivos XML.";
            return false;
        }

        if (file.Length <= 0)
        {
            errorMessage = "El archivo XML está vacío.";
            return false;
        }

        if (file.Length > MaxXmlBytes)
        {
            errorMessage = "El archivo XML excede el tamaño máximo permitido.";
            return false;
        }

        return true;
    }

    private static string EnsureUploadsRoot()
    {
        var root = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        Directory.CreateDirectory(root);
        return root;
    }
}
