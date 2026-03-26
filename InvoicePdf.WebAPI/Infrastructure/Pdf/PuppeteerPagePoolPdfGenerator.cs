using InvoicePdf.WebAPI.Application.Abstractions;
using Microsoft.Extensions.Options;
using PuppeteerPagePool.Abstractions;
using PuppeteerSharp;

namespace InvoicePdf.WebAPI.Infrastructure.Pdf;

public sealed class PuppeteerPagePoolPdfGenerator(IPagePool pagePool, IOptions<PdfRenderOptions> options) : IPdfGenerator
{
    private readonly IPagePool _pagePool = pagePool;
    private readonly PdfRenderOptions _options = options.Value;

    public async Task<byte[]> GenerateAsync(string html, CancellationToken cancellationToken = default)
    {
        var pageSize = ResolvePageSize(_options.PageFormat);

        return await _pagePool.ExecuteAsync(async (page, token) =>
        {
            token.ThrowIfCancellationRequested();
            await page.SetContentAsync(html);
            await page.WaitForNetworkIdleAsync();
            return await page.PdfDataAsync(new PdfOptions
            {
                PrintBackground = true,
                PreferCSSPageSize = true,
                Width = pageSize.Width,
                Height = pageSize.Height
            });
        }, cancellationToken);
    }

    private static (string Width, string Height) ResolvePageSize(string pageFormat)
    {
        return pageFormat.ToUpperInvariant() switch
        {
            "LETTER" => ("8.5in", "11in"),
            _ => ("210mm", "297mm")
        };
    }
}
