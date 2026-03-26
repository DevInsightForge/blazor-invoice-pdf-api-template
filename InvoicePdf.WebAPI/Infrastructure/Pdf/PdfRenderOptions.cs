using System.ComponentModel.DataAnnotations;

namespace InvoicePdf.WebAPI.Infrastructure.Pdf;

public sealed class PdfRenderOptions
{
    public const string SectionName = "PdfRender";

    [Required]
    public string PageFormat { get; init; } = "A4";
}
