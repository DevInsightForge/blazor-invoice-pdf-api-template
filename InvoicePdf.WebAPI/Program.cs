using InvoicePdf.WebAPI.Endpoints;
using InvoicePdf.WebAPI.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInvoicePdfTemplate();
builder.Services.AddApiDocumentation();

var app = builder.Build();

app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseApiDocumentation();
}
app.MapInvoicePdfEndpoint();

app.Run();
