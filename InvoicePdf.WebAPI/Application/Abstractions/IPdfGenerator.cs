using InvoicePdf.WebAPI.Application.Models;

namespace InvoicePdf.WebAPI.Application.Abstractions;

public interface IPdfGenerator
{
    Task<byte[]> GenerateAsync(string html, PdfPageFormat pageFormat, CancellationToken cancellationToken = default);
}
