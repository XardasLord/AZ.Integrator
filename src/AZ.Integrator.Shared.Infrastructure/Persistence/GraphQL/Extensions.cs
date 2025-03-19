using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Invoice;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ParcelTemplate;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Shipment;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Stock;
using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL.Queries;
using HotChocolate.AspNetCore;
using HotChocolate.Types.Pagination;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL;

internal static class Extensions
{
    private const string OptionsSectionName = "Infrastructure:GraphQL";

    public static IServiceCollection AddIntegratorGraphQl(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GraphQlOptions>(configuration.GetRequiredSection(OptionsSectionName));
        
        var graphQlOptions = configuration.GetOptions<GraphQlOptions>(OptionsSectionName);
        
        services
            .AddGraphQLServer()
            .InitializeOnStartup()
            .AddAuthorization()
            .RegisterDbContext<ShipmentDataViewContext>()
            .RegisterDbContext<InvoiceDataViewContext>()
            .RegisterDbContext<TagParcelTemplateDataViewContext>()
            .RegisterDbContext<StockDataViewContext>()
            .AddQueryType(q => q.Name(nameof(IntegratorQuery)))
            .AddType<IntegratorQuery>()
            .AddProjections()
            .AddFiltering()
            .AddSorting()
            .SetPagingOptions(new PagingOptions
            {
                IncludeTotalCount = true,
                MaxPageSize = graphQlOptions.MaxPageSize,
                DefaultPageSize = graphQlOptions.DefaultPageSize
            })
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);
        
        return services;
    }
    
    public static IApplicationBuilder UseIntegratorGraphQl(this IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment env)
    {
        var graphQlOptions = configuration.GetOptions<GraphQlOptions>(OptionsSectionName);
        
        return app.UseEndpoints(x => x.MapGraphQL(graphQlOptions.Endpoint)
            .WithOptions(new GraphQLServerOptions
            {
                Tool =
                {
                    Enable = env.IsDevelopment(),
                    Title = "AZ Integrator Portal GraphQL"
                }
            }));
    }
}