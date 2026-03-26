using Bogus;
using InvoicePdf.Templates.Models;
using InvoicePdf.WebAPI.Application.Abstractions;

namespace InvoicePdf.WebAPI.Domain.Factories;

public sealed class RandomInvoiceDocumentFactory : IInvoiceDocumentFactory
{
    private static readonly decimal[] TaxRates = [0m, 5m, 7.5m, 10m, 12.5m, 15m];

    private readonly Faker _faker = new();
    private readonly Faker<AddressInfoModel> _addressFaker;
    private readonly Faker<PartyInfoModel> _partyFaker;
    private readonly Faker<InvoiceLineItemModel> _lineItemFaker;

    public RandomInvoiceDocumentFactory()
    {
        _addressFaker = new Faker<AddressInfoModel>()
            .CustomInstantiator(f => new AddressInfoModel
            {
                Street1 = f.Address.StreetAddress(),
                Street2 = f.Random.Bool(0.35f) ? f.Address.SecondaryAddress() : null,
                City = f.Address.City(),
                StateOrProvince = f.Address.StateAbbr(),
                PostalCode = f.Address.ZipCode(),
                Country = f.Address.CountryCode()
            });

        _partyFaker = new Faker<PartyInfoModel>()
            .CustomInstantiator(f => new PartyInfoModel
            {
                Name = f.Company.CompanyName(),
                Address = _addressFaker.Generate()
            });

        _lineItemFaker = new Faker<InvoiceLineItemModel>()
            .CustomInstantiator(f =>
            {
                var quantity = Round(f.Random.Decimal(1m, 24m));
                var rate = Round(f.Random.Decimal(35m, 175m));
                var amount = Round(quantity * rate);

                return new InvoiceLineItemModel
                {
                    ProductOrService = f.Commerce.Department(1),
                    Description = f.Commerce.ProductName(),
                    Quantity = quantity,
                    Rate = rate,
                    Amount = f.Random.Bool(0.2f) ? amount : null
                };
            });
    }

    public InvoiceDocumentModel Create()
    {
        var invoiceDate = DateOnly.FromDateTime(_faker.Date.RecentOffset(10).UtcDateTime);
        var dueDate = invoiceDate.AddDays(_faker.Random.Int(7, 45));
        var taxRatePercent = _faker.PickRandom(TaxRates);
        var shipping = Round(_faker.Random.Decimal(0m, 120m));
        var company = CreateCompany();
        var billTo = _partyFaker.Generate();
        var shipTo = _faker.Random.Bool(0.6f) ? _partyFaker.Generate() : null;
        var items = _lineItemFaker.Generate(_faker.Random.Int(5, 8));

        return new InvoiceDocumentModel
        {
            Company = company,
            BillTo = billTo,
            ShipTo = shipTo,
            Details = new InvoiceDetailsModel
            {
                InvoiceNumber = $"INV-{invoiceDate:yyyyMMdd}-{_faker.Random.Number(100000, 999999)}",
                InvoiceDate = invoiceDate,
                Terms = _faker.Random.Bool(0.85f) ? $"Net {_faker.Random.Int(7, 45)}" : null,
                DueDate = dueDate,
                Title = "Invoicing"
            },
            Items = items,
            Totals = new InvoiceTotalsModel
            {
                Subtotal = null,
                TaxRatePercent = taxRatePercent,
                SalesTax = null,
                Shipping = shipping,
                Total = null
            },
            CustomerMessageTitle = "Customer message",
            CustomerMessage = _faker.Random.Bool(0.75f) ? _faker.Lorem.Paragraphs(2, "\n\n") : null,
            CurrencyCode = _faker.Finance.Currency().Code,
            Locale = "en-US"
        };
    }

    private CompanyInfoModel CreateCompany()
    {
        return new CompanyInfoModel
        {
            LogoUrl = null,
            CompanyName = _faker.Company.CompanyName(),
            Address = _addressFaker.Generate(),
            Phone = _faker.Phone.PhoneNumber(),
            Email = _faker.Internet.Email(),
            Website = _faker.Internet.DomainName()
        };
    }

    private static decimal Round(decimal value)
    {
        return decimal.Round(value, 2, MidpointRounding.ToEven);
    }
}
