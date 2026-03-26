using InvoicePdf.WebAPI.Application.Abstractions;
using PuppeteerPagePool.Abstractions;
using PuppeteerSharp;

namespace InvoicePdf.WebAPI.Infrastructure.Pdf;

public sealed class PuppeteerPagePoolPdfGenerator(IPagePool pagePool) : IPdfGenerator
{
    private readonly IPagePool _pagePool = pagePool;

    public async Task<byte[]> GenerateAsync(string html, CancellationToken cancellationToken = default)
    {
        return await _pagePool.ExecuteAsync(async (page, token) =>
        {
            token.ThrowIfCancellationRequested();
            await page.SetContentAsync(html);
            await page.WaitForNetworkIdleAsync();
            return await page.PdfDataAsync(new PdfOptions
            {
                PrintBackground = true,
                PreferCSSPageSize = true,
                Width = "210mm",
                Height = "297mm"
            });
        }, cancellationToken);
    }
}
