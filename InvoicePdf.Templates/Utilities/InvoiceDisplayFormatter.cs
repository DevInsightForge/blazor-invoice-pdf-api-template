using System.Globalization;
using InvoicePdf.Templates.Models;

namespace InvoicePdf.Templates.Utilities;

public sealed class InvoiceDisplayFormatter
{
    private readonly CultureInfo _culture;
    private readonly string _currencyCode;

    public InvoiceDisplayFormatter(string? locale, string? currencyCode)
    {
        _culture = ResolveCulture(locale);
        _currencyCode = string.IsNullOrWhiteSpace(currencyCode) ? "USD" : currencyCode;
    }

    public string FormatCurrency(decimal value)
    {
        return $"{_currencyCode} {value.ToString("N2", _culture)}";
    }

    public string FormatDate(DateOnly? value)
    {
        return value.HasValue ? value.Value.ToString("d", _culture) : "-";
    }

    public string FormatNumber(decimal value)
    {
        return value.ToString("0.##", _culture);
    }

    public string FormatAddress(AddressInfoModel address)
    {
        var parts = new List<string>();

        if (!string.IsNullOrWhiteSpace(address.Street1))
        {
            parts.Add(address.Street1);
        }

        if (!string.IsNullOrWhiteSpace(address.Street2))
        {
            parts.Add(address.Street2);
        }

        var cityState = string.Join(", ", new[] { address.City, address.StateOrProvince }.Where(x => !string.IsNullOrWhiteSpace(x)));
        if (!string.IsNullOrWhiteSpace(address.PostalCode))
        {
            cityState = string.IsNullOrWhiteSpace(cityState) ? address.PostalCode : $"{cityState} {address.PostalCode}";
        }

        if (!string.IsNullOrWhiteSpace(cityState))
        {
            parts.Add(cityState);
        }

        if (!string.IsNullOrWhiteSpace(address.Country))
        {
            parts.Add(address.Country);
        }

        return parts.Count == 0 ? "-" : string.Join("\n", parts);
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
