using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL.Queries;
using AZ.Integrator.Shared.Infrastructure.UtilityExtensions;
using HotChocolate.AspNetCore;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types.Pagination;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL;

public static class Extensions
{
    private const string OptionsSectionName = "Infrastructure:GraphQL";

    public static IRequestExecutorBuilder AddIntegratorGraphQl(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GraphQlOptions>(configuration.GetRequiredSection(OptionsSectionName));
        
        var graphQlOptions = configuration.GetOptions<GraphQlOptions>(OptionsSectionName);
        
        return services
            .AddGraphQLServer()
            .InitializeOnStartup()
            .AddAuthorization()
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