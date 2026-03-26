using Swashbuckle.AspNetCore.SwaggerUI;

namespace InvoicePdf.WebAPI.Infrastructure.DependencyInjection;

public static class OpenApiExtensions
{
    public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
    {
        services.AddOpenApi();
        return services;
    }

    public static IApplicationBuilder UseApiDocumentation(this WebApplication app)
    {
        app.MapOpenApi();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "v1");
            options.RoutePrefix = "swagger";
            options.DocExpansion(DocExpansion.List);
        });

        return app;
    }
}
