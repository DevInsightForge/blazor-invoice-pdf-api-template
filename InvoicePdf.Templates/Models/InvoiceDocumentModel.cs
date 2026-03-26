namespace InvoicePdf.Templates.Models;

public sealed class InvoiceDocumentModel
{
    public CompanyInfoModel Company { get; init; } = new();
    public InvoiceMetaModel Invoice { get; init; } = new();
    public BillToInfoModel BillTo { get; init; } = new();
    public OrderDetailsModel OrderDetails { get; init; } = new();
    public IReadOnlyList<InvoiceLineItemModel> LineItems { get; init; } = [];
    public InvoiceSummaryModel Summary { get; init; } = new();
    public PaymentReceiptModel PaymentReceipt { get; init; } = new();
    public InvoiceFooterModel Footer { get; init; } = new();
    public string CurrencyCode { get; init; } = "USD";
    public string Locale { get; init; } = "en-US";
    public string InvoiceNumber => Invoice.InvoiceNumber;
}

public sealed class CompanyInfoModel
{
    public string Eyebrow { get; init; } = "Paid Invoice";
    public string CompanyName { get; init; } = string.Empty;
    public string AddressLine { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
}

public sealed class InvoiceMetaModel
{
    public string InvoiceNumber { get; init; } = string.Empty;
    public DateOnly? IssueDate { get; init; }
    public string PaymentStatus { get; init; } = "Paid";
}

public sealed class BillToInfoModel
{
    public string CardLabel { get; init; } = "Bill To";
    public string Name { get; init; } = string.Empty;
    public string? Attention { get; init; }
    public string AddressLine1 { get; init; } = string.Empty;
    public string? AddressLine2 { get; init; }
    public string CityStatePostalCountry { get; init; } = string.Empty;
    public string? Email { get; init; }
}

public sealed class OrderDetailsModel
{
    public string CardLabel { get; init; } = "Order Details";
    public string Name { get; init; } = string.Empty;
    public IReadOnlyList<InvoiceKeyValueModel> Lines { get; init; } = [];
}

public sealed class InvoiceKeyValueModel
{
    public string Label { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
}

public sealed class InvoiceLineItemModel
{
    public string Id { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal? Amount { get; init; }
}

public sealed class InvoiceSummaryModel
{
    public decimal? Subtotal { get; init; }
    public decimal? TaxRatePercent { get; init; }
    public decimal? Tax { get; init; }
    public decimal Shipping { get; init; }
    public decimal Discount { get; init; }
    public decimal? Total { get; init; }
    public decimal? AmountPaid { get; init; }
    public decimal? Balance { get; init; }
}

public sealed class PaymentReceiptModel
{
    public string CardLabel { get; init; } = "Payment Receipt";
    public IReadOnlyList<InvoiceKeyValueModel> Lines { get; init; } = [];
}

public sealed class InvoiceFooterModel
{
    public string Line1 { get; init; } = string.Empty;
    public string Line2 { get; init; } = string.Empty;
}
