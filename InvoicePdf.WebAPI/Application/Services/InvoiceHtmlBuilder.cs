using InvoicePdf.Templates.Models;
using InvoicePdf.Templates.Rendering;
using InvoicePdf.WebAPI.Application.Abstractions;

namespace InvoicePdf.WebAPI.Application.Services;

public sealed class InvoiceHtmlBuilder(IComponentHtmlRenderer componentHtmlRenderer) : IInvoiceHtmlBuilder
{
    private readonly IComponentHtmlRenderer _componentHtmlRenderer = componentHtmlRenderer;

    public async Task<string> BuildAsync(InvoiceDocumentModel document, CancellationToken cancellationToken = default)
    {
        var body = await _componentHtmlRenderer.RenderInvoiceDocumentAsync(document, cancellationToken);
        return InvoiceHtmlDocumentComposer.Compose(body, document.InvoiceNumber);
    }
}
