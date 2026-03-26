using System.Globalization;
using InvoicePdf.Templates.Models;
using InvoicePdf.WebAPI.Application.Abstractions;

namespace InvoicePdf.WebAPI.Domain.Factories;

public sealed class RandomInvoiceDocumentFactory : IInvoiceDocumentFactory
{
    public InvoiceDocumentModel Create()
    {
        var issueDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-Random.Shared.Next(0, 7));
        var dueDate = issueDate.AddDays(Random.Shared.Next(7, 45));
        var currencyCode = CreateUpperToken(3);
        var items = CreateLineItems();
        var subtotal = Round(items.Sum(x => x.LineSubtotal));
        var taxTotal = Round(items.Sum(x => x.LineTax));
        var grandTotal = Round(subtotal + taxTotal);

        return new InvoiceDocumentModel(
            InvoiceNumber: $"INV-{issueDate:yyyyMMdd}-{Random.Shared.Next(100000, 999999)}",
            IssueDate: issueDate,
            DueDate: dueDate,
            CurrencyCode: currencyCode,
            Seller: CreateParty("SELLER"),
            Buyer: CreateParty("BUYER"),
            LineItems: items,
            Subtotal: subtotal,
            TaxTotal: taxTotal,
            GrandTotal: grandTotal,
            PaymentTerms: $"Net {Random.Shared.Next(7, 46)}",
            Notes: $"Generated at {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)} UTC");
    }

    private static InvoicePartyModel CreateParty(string role)
    {
        var nameToken = CreateUpperToken(6);
        return new InvoicePartyModel(
            Name: $"{role}-{CreateUpperToken(4)} {nameToken}",
            AddressLine1: $"{Random.Shared.Next(10, 9999)} {CreateUpperToken(8)} Street",
            AddressLine2: $"Suite {Random.Shared.Next(1, 90)}",
            City: $"City-{CreateUpperToken(5)}",
            Country: $"Country-{CreateUpperToken(3)}",
            Email: $"{role.ToLowerInvariant()}-{nameToken.ToLowerInvariant()}@{CreateLowerToken(5)}.{CreateLowerToken(3)}");
    }

    private static IReadOnlyList<InvoiceLineItemModel> CreateLineItems()
    {
        var count = Random.Shared.Next(2, 7);
        var items = new List<InvoiceLineItemModel>(count);

        for (var index = 0; index < count; index++)
        {
            var quantity = Round((decimal)(Random.Shared.NextDouble() * 20d) + 1m);
            var unitPrice = Round((decimal)(Random.Shared.NextDouble() * 1200d) + 50m);
            var taxRate = Random.Shared.Next(0, 4) * 5m;
            var subtotal = Round(quantity * unitPrice);
            var tax = Round(subtotal * (taxRate / 100m));
            var total = Round(subtotal + tax);

            items.Add(new InvoiceLineItemModel(
                Description: $"Service-{CreateUpperToken(5)}-{index + 1}",
                Quantity: quantity,
                UnitPrice: unitPrice,
                TaxRate: taxRate,
                LineSubtotal: subtotal,
                LineTax: tax,
                LineTotal: total));
        }

        return items;
    }

    private static string CreateUpperToken(int length)
    {
        return string.Create(length, 0, static (span, _) =>
        {
            for (var index = 0; index < span.Length; index++)
            {
                span[index] = (char)('A' + Random.Shared.Next(0, 26));
            }
        });
    }

    private static string CreateLowerToken(int length)
    {
        return string.Create(length, 0, static (span, _) =>
        {
            for (var index = 0; index < span.Length; index++)
            {
                span[index] = (char)('a' + Random.Shared.Next(0, 26));
            }
        });
    }

    private static decimal Round(decimal value)
    {
        return decimal.Round(value, 2, MidpointRounding.ToEven);
    }
}
