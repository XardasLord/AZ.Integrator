using AZ.Integrator.Api.Infrastructure;
using AZ.Integrator.Catalog.Infrastructure;
using AZ.Integrator.Invoices.Infrastructure;
using AZ.Integrator.Monitoring.Infrastructure;
using AZ.Integrator.Orders.Infrastructure;
using AZ.Integrator.Shipments.Infrastructure;
using AZ.Integrator.Stocks.Infrastructure;
using AZ.Integrator.TagParcelTemplates.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseApiInfrastructure(builder.Configuration, app.Environment);
app.MapInvoicesModuleEndpoints();
app.MapShipmentsModuleEndpoints();
app.MapOrdersModuleEndpoints();
app.MapTagParcelTemplatesModuleEndpoints();
app.MapStocksModuleEndpoints();
app.MapMonitoringModuleEndpoints();
app.MapCatalogModuleEndpoints();

app.MapGet("/", () => "Hello from HTTPS!");
app.MapGet("/cert", () => Results.File(Path.Combine(Directory.GetCurrentDirectory(), "https", "cert.pfx"), "application/x-pkcs12"));

app.Run();
