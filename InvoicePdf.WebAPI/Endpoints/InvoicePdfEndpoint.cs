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
            .WithTags("Invoices")
            .WithSummary("Generate invoice PDF")
            .WithDescription("Generates a randomized invoice using the template and returns it as an A4 PDF.")
            .Produces(StatusCodes.Status200OK, contentType: "application/pdf")
            .ProducesProblem(StatusCodes.Status503ServiceUnavailable)
            .ProducesProblem(StatusCodes.Status504GatewayTimeout);

        return endpoints;
    }

    private static async Task<Results<FileContentHttpResult, ProblemHttpResult>> GeneratePdfAsync(
        InvoicePdfService service,
        CancellationToken cancellationToken)
    {
        try
        {
            var (pdf, fileName) = await service.GenerateAsync(cancellationToken);
            return TypedResults.File(pdf, "application/pdf", fileName);
        }
        catch (Exception ex) when (IsPoolUnavailable(ex))
        {
            return TypedResults.Problem(
                title: "PDF engine unavailable",
                detail: ex.Message,
                statusCode: StatusCodes.Status503ServiceUnavailable);
        }
        catch (Exception ex) when (IsAcquireTimeout(ex))
        {
            return TypedResults.Problem(
                title: "PDF generation timeout",
                detail: ex.Message,
                statusCode: StatusCodes.Status504GatewayTimeout);
        }
    }

    private static bool IsPoolUnavailable(Exception exception)
    {
        var name = exception.GetType().Name;
        return name is "PagePoolUnavailableException" or "PagePoolCircuitOpenException" or "PagePoolDisposedException";
    }

    private static bool IsAcquireTimeout(Exception exception)
    {
        return exception.GetType().Name == "PagePoolAcquireTimeoutException";
    }
}
