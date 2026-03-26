using InvoicePdf.Templates.Models;

namespace InvoicePdf.WebAPI.Application.Abstractions;

public interface IInvoiceHtmlBuilder
{
    Task<string> BuildAsync(InvoiceDocumentModel document, CancellationToken cancellationToken = default);
}
