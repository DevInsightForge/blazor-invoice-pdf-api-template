namespace InvoicePdf.Templates.Models;

public sealed class InvoiceDocumentModel
{
    public CompanyInfoModel Company { get; init; } = new();
    public PartyInfoModel BillTo { get; init; } = new();
    public PartyInfoModel? ShipTo { get; init; }
    public InvoiceDetailsModel Details { get; init; } = new();
    public IReadOnlyList<InvoiceLineItemModel> Items { get; init; } = [];
    public InvoiceTotalsModel Totals { get; init; } = new();
    public string? CustomerMessageTitle { get; init; } = "Customer message";
    public string? CustomerMessage { get; init; }
    public string CurrencyCode { get; init; } = "USD";
    public string Locale { get; init; } = "en-US";
    public string InvoiceNumber => Details.InvoiceNumber;
}

public sealed class CompanyInfoModel
{
    public string? LogoUrl { get; init; }
    public string CompanyName { get; init; } = string.Empty;
    public AddressInfoModel Address { get; init; } = new();
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public string? Website { get; init; }
}

public sealed class PartyInfoModel
{
    public string Name { get; init; } = string.Empty;
    public AddressInfoModel Address { get; init; } = new();
}

public sealed class AddressInfoModel
{
    public string? Street1 { get; init; }
    public string? Street2 { get; init; }
    public string? City { get; init; }
    public string? StateOrProvince { get; init; }
    public string? PostalCode { get; init; }
    public string? Country { get; init; }
}

public sealed class InvoiceDetailsModel
{
    public string InvoiceNumber { get; init; } = string.Empty;
    public DateOnly? InvoiceDate { get; init; }
    public string? Terms { get; init; }
    public DateOnly? DueDate { get; init; }
    public string Title { get; init; } = "Invoicing";
}

public sealed class InvoiceLineItemModel
{
    public string ProductOrService { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal Quantity { get; init; }
    public decimal Rate { get; init; }
    public decimal? Amount { get; init; }
}

public sealed class InvoiceTotalsModel
{
    public decimal? Subtotal { get; init; }
    public decimal? TaxRatePercent { get; init; }
    public decimal? SalesTax { get; init; }
    public decimal Shipping { get; init; }
    public decimal? Total { get; init; }
}
