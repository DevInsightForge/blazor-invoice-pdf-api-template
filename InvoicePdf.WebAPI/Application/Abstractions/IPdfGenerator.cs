namespace InvoicePdf.WebAPI.Application.Abstractions;

public interface IPdfGenerator
{
    Task<byte[]> GenerateAsync(string html, CancellationToken cancellationToken = default);
}
