using Bogus;
using InvoicePdf.Templates.Models;
using InvoicePdf.WebAPI.Application.Abstractions;

namespace InvoicePdf.WebAPI.Domain.Factories;

public sealed class RandomInvoiceDocumentFactory : IInvoiceDocumentFactory
{
    private readonly Faker _faker = new();

    public InvoiceDocumentModel Create()
    {
        var issueDate = DateOnly.FromDateTime(_faker.Date.RecentOffset(7).UtcDateTime);
        var shippingDate = issueDate.AddDays(_faker.Random.Int(1, 4));
        var deliveryDate = shippingDate.AddDays(_faker.Random.Int(1, 4));
        var lineItems = CreateLineItems(_faker.Random.Int(4, 7));
        var subtotal = Round(lineItems.Sum(x => x.Amount ?? (x.Quantity * x.UnitPrice)));
        var taxRate = _faker.PickRandom(new[] { 8.25m, 10m, 12m });
        var tax = Round(subtotal * (taxRate / 100m));
        var shipping = Round(_faker.Random.Decimal(15m, 60m));
        var discount = Round(_faker.Random.Decimal(10m, 180m));
        var total = Round(subtotal + tax + shipping - discount);
        var amountPaid = _faker.Random.Bool(0.8f) ? total : Round(total - _faker.Random.Decimal(10m, 150m));
        var balance = Round(total - amountPaid);
        var invoiceNumber = $"INV-{issueDate:yyyy}-{_faker.Random.Number(1000, 9999)}";

        return new InvoiceDocumentModel
        {
            Company = new CompanyInfoModel
            {
                Eyebrow = _faker.PickRandom(new[] { "Paid Invoice", "Invoice" }),
                CompanyName = _faker.Company.CompanyName(),
                AddressLine = BuildAddressLine(),
                Email = _faker.Internet.Email(),
                Phone = _faker.Phone.PhoneNumber("+1 (###) ###-####")
            },
            Invoice = new InvoiceMetaModel
            {
                InvoiceNumber = invoiceNumber,
                IssueDate = issueDate,
                PaymentStatus = balance == 0m ? "Paid" : "Partial"
            },
            BillTo = new BillToInfoModel
            {
                CardLabel = "Bill To",
                Name = _faker.Company.CompanyName(),
                Attention = $"Attn: {_faker.Name.JobTitle()}",
                AddressLine1 = _faker.Address.StreetAddress(),
                AddressLine2 = _faker.Random.Bool(0.4f) ? _faker.Address.SecondaryAddress() : null,
                CityStatePostalCountry = BuildCityStateLine(),
                Email = _faker.Internet.Email()
            },
            OrderDetails = new OrderDetailsModel
            {
                CardLabel = "Order Details",
                Name = _faker.Company.CompanyName(),
                Lines =
                [
                    new InvoiceKeyValueModel { Label = "Order Number", Value = $"ORD-{_faker.Random.Number(100000, 999999)}" },
                    new InvoiceKeyValueModel { Label = "Order Date", Value = _faker.Date.Recent(10).ToString("MMMM dd, yyyy") },
                    new InvoiceKeyValueModel { Label = "Shipping Method", Value = _faker.PickRandom(new[] { "Express Ground", "Priority", "Standard" }) },
                    new InvoiceKeyValueModel { Label = "Tracking", Value = $"TRK-{_faker.Random.Number(1000000000, 1999999999)}" },
                    new InvoiceKeyValueModel { Label = "Delivered", Value = deliveryDate.ToString("MMMM dd, yyyy") }
                ]
            },
            LineItems = lineItems,
            Summary = new InvoiceSummaryModel
            {
                Subtotal = subtotal,
                TaxRatePercent = taxRate,
                Tax = tax,
                Shipping = shipping,
                Discount = discount,
                Total = total,
                AmountPaid = amountPaid,
                Balance = balance
            },
            PaymentReceipt = new PaymentReceiptModel
            {
                CardLabel = "Payment Receipt",
                Lines =
                [
                    new InvoiceKeyValueModel { Label = "Payment Method", Value = _faker.PickRandom(new[] { "Corporate Credit Account", "Wire Transfer", "Business Card" }) },
                    new InvoiceKeyValueModel { Label = "Card Type", Value = _faker.PickRandom(new[] { "Visa Business", "Mastercard Business", "Amex Corporate" }) },
                    new InvoiceKeyValueModel { Label = "Last 4 Digits", Value = _faker.Random.Number(1000, 9999).ToString() },
                    new InvoiceKeyValueModel { Label = "Authorization", Value = $"AUTH-{_faker.Random.Number(100000, 999999)}" },
                    new InvoiceKeyValueModel { Label = "Transaction ID", Value = $"TXN-{issueDate:yyyyMMdd}-{_faker.Random.Number(1000, 9999)}" },
                    new InvoiceKeyValueModel { Label = "Reference", Value = invoiceNumber }
                ]
            },
            Footer = new InvoiceFooterModel
            {
                Line1 = balance == 0m
                    ? $"Payment received on {issueDate:MMMM dd, yyyy}. This invoice is settled and no outstanding balance remains."
                    : $"Partial payment recorded on {issueDate:MMMM dd, yyyy}. Remaining balance is due by stated terms.",
                Line2 = $"{_faker.Company.CompanyName()} LLC · EIN {_faker.Random.Number(10, 99)}-{_faker.Random.Number(1000000, 9999999)}"
            },
            CurrencyCode = "USD",
            Locale = "en-US"
        };
    }

    private List<InvoiceLineItemModel> CreateLineItems(int count)
    {
        var items = new List<InvoiceLineItemModel>(count);

        for (var i = 1; i <= count; i++)
        {
            var quantity = Round(_faker.Random.Decimal(1m, 5m));
            var unitPrice = Round(_faker.Random.Decimal(89m, 599m));
            var amount = Round(quantity * unitPrice);

            items.Add(new InvoiceLineItemModel
            {
                Id = i.ToString("00"),
                Description = _faker.Commerce.ProductName(),
                Quantity = quantity,
                UnitPrice = unitPrice,
                Amount = _faker.Random.Bool(0.75f) ? amount : null
            });
        }

        return items;
    }

    private string BuildAddressLine()
    {
        return $"{_faker.Address.StreetAddress()}, {_faker.Address.City()}, {_faker.Address.StateAbbr()} {_faker.Address.ZipCode()}";
    }

    private string BuildCityStateLine()
    {
        return $"{_faker.Address.City()}, {_faker.Address.StateAbbr()} {_faker.Address.ZipCode()}, {_faker.Address.Country()}";
    }

    private static decimal Round(decimal value)
    {
        return decimal.Round(value, 2, MidpointRounding.ToEven);
    }
}
