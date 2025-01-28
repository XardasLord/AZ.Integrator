using AZ.Integrator.Invoices.Application;
using AZ.Integrator.Orders.Application;
using AZ.Integrator.TagParcelTemplates.Application;
using AZ.Integrator.Shared.Infrastructure;
using AZ.Integrator.Shipments.Application;
using AZ.Integrator.Stocks.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOrdersModuleApplication(builder.Configuration);
builder.Services.AddShipmentsModuleApplication(builder.Configuration);
builder.Services.AddInvoicesModuleApplication(builder.Configuration);
builder.Services.AddTagParcelTemplatesModuleApplication(builder.Configuration);
builder.Services.AddStocksModuleApplication(builder.Configuration);

builder.Services.AddSharedInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseInfrastructure(builder.Configuration, app.Environment);

app.MapGet("/", (HttpContext ctx) =>
{
    return ctx.User.Claims.Select(x => new { x.Type, x.Value }).ToList();
});

app.Run();
