using InvoicePdf.Templates.Models;

namespace InvoicePdf.WebAPI.Application.Abstractions;

public interface IInvoiceDocumentFactory
{
    InvoiceDocumentModel Create();
}
