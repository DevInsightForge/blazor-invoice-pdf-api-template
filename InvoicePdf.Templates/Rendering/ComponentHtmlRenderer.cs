using InvoicePdf.Templates.Components.Invoice;
using InvoicePdf.Templates.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;

namespace InvoicePdf.Templates.Rendering;

public interface IComponentHtmlRenderer
{
    Task<string> RenderInvoiceDocumentAsync(InvoiceDocumentModel model, CancellationToken cancellationToken = default);
}

public sealed class ComponentHtmlRenderer : IComponentHtmlRenderer, IAsyncDisposable
{
    private readonly HtmlRenderer _renderer;

    public ComponentHtmlRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        _renderer = new HtmlRenderer(serviceProvider, loggerFactory);
    }

    public Task<string> RenderInvoiceDocumentAsync(InvoiceDocumentModel model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return _renderer.RenderComponent<InvoiceDocument>(new Dictionary<string, object?>
        {
            [nameof(InvoiceDocument.Model)] = model
        });
    }

    public ValueTask DisposeAsync()
    {
        return _renderer.DisposeAsync();
    }
}
