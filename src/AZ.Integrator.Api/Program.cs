using AZ.Integrator.Api.Infrastructure;
using AZ.Integrator.Orders.Infrastructure;
using AZ.Integrator.Stocks.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseApiInfrastructure(builder.Configuration, app.Environment);
app.MapStocksModuleEndpoints();
app.MapOrdersModuleEndpoints();

app.MapGet("/", () => "Hello from HTTPS!");
app.MapGet("/cert", () => Results.File(Path.Combine(Directory.GetCurrentDirectory(), "https", "cert.pfx"), "application/x-pkcs12"));

app.Run();
