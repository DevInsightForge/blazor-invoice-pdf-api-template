using InvoicePdf.Templates.Rendering;
using InvoicePdf.WebAPI.Application.Abstractions;
using InvoicePdf.WebAPI.Application.Services;
using InvoicePdf.WebAPI.Domain.Factories;
using InvoicePdf.WebAPI.Infrastructure.Pdf;
using Microsoft.Extensions.Options;
using PuppeteerPagePool;
using PuppeteerSharp;

namespace InvoicePdf.WebAPI.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInvoicePdfTemplate(this IServiceCollection services)
    {
        services.AddSingleton<IComponentHtmlRenderer, ComponentHtmlRenderer>();
        services.AddOptions<PdfRenderOptions>()
            .BindConfiguration(PdfRenderOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddTransient<IInvoiceDocumentFactory, RandomInvoiceDocumentFactory>();
        services.AddSingleton<IInvoiceHtmlBuilder, InvoiceHtmlBuilder>();
        services.AddSingleton<InvoicePdfService>();
        services.AddSingleton<IPdfGenerator, PuppeteerPagePoolPdfGenerator>();

        services.AddPuppeteerPagePool(options =>
        {
            options.PoolSize = 4;
            options.AcquireTimeout = TimeSpan.FromSeconds(20);
            options.ShutdownTimeout = TimeSpan.FromSeconds(20);
            options.ResetTargetUrl = "about:blank";
            options.LaunchOptions = new LaunchOptions
            {
                Headless = true,
                Timeout = 120000,
                Args =
                [
                    "--disable-dev-shm-usage",
                    "--disable-gpu",
                    "--no-sandbox"
                ]
            };
        });

        return services;
    }
}
