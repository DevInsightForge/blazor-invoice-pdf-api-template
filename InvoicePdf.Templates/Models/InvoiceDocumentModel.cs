namespace InvoicePdf.Templates.Models;

public sealed record InvoiceDocumentModel(
    string InvoiceNumber,
    DateOnly IssueDate,
    DateOnly DueDate,
    string CurrencyCode,
    InvoicePartyModel Seller,
    InvoicePartyModel Buyer,
    IReadOnlyList<InvoiceLineItemModel> LineItems,
    decimal Subtotal,
    decimal TaxTotal,
    decimal GrandTotal,
    string? PaymentTerms,
    string? Notes);

public sealed record InvoicePartyModel(
    string Name,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string Country,
    string Email);

public sealed record InvoiceLineItemModel(
    string Description,
    decimal Quantity,
    decimal UnitPrice,
    decimal TaxRate,
    decimal LineSubtotal,
    decimal LineTax,
    decimal LineTotal);
