using InvoicePdf.WebAPI.Application.Abstractions;
using InvoicePdf.WebAPI.Application.Models;
using PuppeteerPagePool.Abstractions;
using PuppeteerSharp;

namespace InvoicePdf.WebAPI.Infrastructure.Pdf;

public sealed class PuppeteerPagePoolPdfGenerator(IPagePool pagePool) : IPdfGenerator
{
    private readonly IPagePool _pagePool = pagePool;

    public async Task<byte[]> GenerateAsync(string html, PdfPageFormat pageFormat, CancellationToken cancellationToken = default)
    {
        var pageSize = ResolvePageSize(pageFormat);

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

    private static (string Width, string Height) ResolvePageSize(PdfPageFormat pageFormat)
    {
        return pageFormat switch
        {
            PdfPageFormat.Letter => ("8.5in", "11in"),
            _ => ("210mm", "297mm")
        };
    }
}
