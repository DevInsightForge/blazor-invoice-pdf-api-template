using InvoicePdf.Templates.Models;

namespace InvoicePdf.Templates.Utilities;

public static class InvoiceCalculations
{
    public static decimal GetLineAmount(InvoiceLineItemModel item)
    {
        return Round(item.Amount ?? (item.Quantity * item.Rate));
    }

    public static decimal GetSubtotal(InvoiceDocumentModel document)
    {
        return Round(document.Totals.Subtotal ?? document.Items.Sum(GetLineAmount));
    }

    public static decimal GetSalesTax(InvoiceDocumentModel document, decimal subtotal)
    {
        if (document.Totals.SalesTax.HasValue)
        {
            return Round(document.Totals.SalesTax.Value);
        }

        if (document.Totals.TaxRatePercent.HasValue)
        {
            return Round(subtotal * document.Totals.TaxRatePercent.Value / 100m);
        }

        return 0m;
    }

    public static decimal GetShipping(InvoiceDocumentModel document)
    {
        return Round(document.Totals.Shipping);
    }

    public static decimal GetTotal(InvoiceDocumentModel document, decimal subtotal, decimal salesTax, decimal shipping)
    {
        return Round(document.Totals.Total ?? (subtotal + salesTax + shipping));
    }

    private static decimal Round(decimal value)
    {
        return decimal.Round(value, 2, MidpointRounding.ToEven);
    }
}
