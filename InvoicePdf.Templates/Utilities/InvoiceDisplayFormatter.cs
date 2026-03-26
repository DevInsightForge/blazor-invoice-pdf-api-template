using System.Globalization;

namespace InvoicePdf.Templates.Utilities;

public sealed class InvoiceDisplayFormatter
{
    private readonly CultureInfo _culture;

    public InvoiceDisplayFormatter(string? locale)
    {
        _culture = ResolveCulture(locale);
    }

    public string FormatCurrency(decimal value)
    {
        return value.ToString("C", _culture);
    }

    public string FormatDate(DateOnly? value)
    {
        return value.HasValue ? value.Value.ToString("MMMM dd, yyyy", _culture) : "-";
    }

    public string FormatNumber(decimal value)
    {
        return value.ToString("0.##", _culture);
    }

    public string SafeText(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? "-" : value;
    }

    private static CultureInfo ResolveCulture(string? locale)
    {
        if (string.IsNullOrWhiteSpace(locale))
        {
            return CultureInfo.GetCultureInfo("en-US");
        }

        try
        {
            return CultureInfo.GetCultureInfo(locale);
        }
        catch (CultureNotFoundException)
        {
            return CultureInfo.GetCultureInfo("en-US");
        }
    }
}
