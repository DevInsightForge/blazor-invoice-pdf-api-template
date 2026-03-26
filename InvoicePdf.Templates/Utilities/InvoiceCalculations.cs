using InvoicePdf.Templates.Models;

namespace InvoicePdf.Templates.Utilities;

public static class InvoiceCalculations
{
    public static decimal GetLineAmount(InvoiceLineItemModel item)
    {
        return Round(item.Amount ?? (item.Quantity * item.UnitPrice));
    }

    public static decimal GetSubtotal(InvoiceDocumentModel document)
    {
        return Round(document.Summary.Subtotal ?? document.LineItems.Sum(GetLineAmount));
    }

    public static decimal GetTax(InvoiceDocumentModel document, decimal subtotal)
    {
        if (document.Summary.Tax.HasValue)
        {
            return Round(document.Summary.Tax.Value);
        }

        if (document.Summary.TaxRatePercent.HasValue)
        {
            return Round(subtotal * document.Summary.TaxRatePercent.Value / 100m);
        }

        return 0m;
    }

    public static decimal GetShipping(InvoiceDocumentModel document)
    {
        return Round(document.Summary.Shipping);
    }

    public static decimal GetDiscount(InvoiceDocumentModel document)
    {
        return Round(document.Summary.Discount);
    }

    public static decimal GetTotal(InvoiceDocumentModel document, decimal subtotal, decimal tax, decimal shipping, decimal discount)
    {
        return Round(document.Summary.Total ?? (subtotal + tax + shipping - discount));
    }

    public static decimal GetAmountPaid(InvoiceDocumentModel document, decimal total)
    {
        return Round(document.Summary.AmountPaid ?? total);
    }

    public static decimal GetBalance(InvoiceDocumentModel document, decimal total, decimal amountPaid)
    {
        return Round(document.Summary.Balance ?? (total - amountPaid));
    }

    private static decimal Round(decimal value)
    {
        return decimal.Round(value, 2, MidpointRounding.ToEven);
    }
}
