using InvoicePdf.WebAPI.Endpoints;
using InvoicePdf.WebAPI.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInvoicePdfTemplate();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapInvoicePdfEndpoint();

app.Run();
