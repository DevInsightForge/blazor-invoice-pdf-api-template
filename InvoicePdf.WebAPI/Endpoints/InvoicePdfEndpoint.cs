using InvoicePdf.WebAPI.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InvoicePdf.WebAPI.Endpoints;

public static class InvoicePdfEndpoint
{
    public static IEndpointRouteBuilder MapInvoicePdfEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet("/api/invoices/pdf", GeneratePdfAsync)
            .WithName("GenerateRandomInvoicePdf")
            .Produces(StatusCodes.Status200OK, contentType: "application/pdf");

        return endpoints;
    }

    private static async Task<FileContentHttpResult> GeneratePdfAsync(InvoicePdfService service, CancellationToken cancellationToken)
    {
        var (pdf, fileName) = await service.GenerateAsync(cancellationToken);
        return TypedResults.File(pdf, "application/pdf", fileName);
    }
}
