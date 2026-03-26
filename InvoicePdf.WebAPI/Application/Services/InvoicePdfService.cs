using InvoicePdf.Templates.Models;
using InvoicePdf.WebAPI.Application.Abstractions;
using InvoicePdf.WebAPI.Application.Models;

namespace InvoicePdf.WebAPI.Application.Services;

public sealed class InvoicePdfService(
    IInvoiceDocumentFactory invoiceDocumentFactory,
    IInvoiceHtmlBuilder invoiceHtmlBuilder,
    IPdfGenerator pdfGenerator)
{
    private readonly IInvoiceDocumentFactory _invoiceDocumentFactory = invoiceDocumentFactory;
    private readonly IInvoiceHtmlBuilder _invoiceHtmlBuilder = invoiceHtmlBuilder;
    private readonly IPdfGenerator _pdfGenerator = pdfGenerator;

    public async Task<(byte[] Pdf, string FileName)> GenerateAsync(PdfPageFormat pageFormat, CancellationToken cancellationToken)
    {
        var document = _invoiceDocumentFactory.Create();
        var html = await _invoiceHtmlBuilder.BuildAsync(document, cancellationToken);
        var pdf = await _pdfGenerator.GenerateAsync(html, pageFormat, cancellationToken);
        return (pdf, BuildFileName(document));
    }

    private static string BuildFileName(InvoiceDocumentModel document)
    {
        var safeInvoiceNumber = string.Concat(document.Details.InvoiceNumber.Where(char.IsLetterOrDigit));
        var invoiceNumber = string.IsNullOrWhiteSpace(safeInvoiceNumber) ? "document" : safeInvoiceNumber;
        return $"invoice-{invoiceNumber}.pdf";
    }
}
