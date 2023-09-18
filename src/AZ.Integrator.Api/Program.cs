using AZ.Integrator.Application;
using AZ.Integrator.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseInfrastructure(builder.Configuration, app.Environment);

app.MapGet("/", (HttpContext ctx) =>
{
    return ctx.User.Claims.Select(x => new { x.Type, x.Value }).ToList();
});

app.Run();
