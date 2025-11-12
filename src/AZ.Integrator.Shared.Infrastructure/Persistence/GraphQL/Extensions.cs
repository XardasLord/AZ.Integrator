using AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL.Queries;
using AZ.Integrator.Shared.Infrastructure.UtilityExtensions;
using HotChocolate.AspNetCore;
using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
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
            .ModifyCostOptions(opt =>
            {
                opt.EnforceCostLimits = false;
            })
            .ModifyPagingOptions(x =>
            {
                x.IncludeTotalCount = true;
                x.MaxPageSize = graphQlOptions.MaxPageSize;
                x.DefaultPageSize = graphQlOptions.DefaultPageSize;
            })
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);
    }
    
    public static IEndpointConventionBuilder MapIntegratorGraphQl(this IEndpointRouteBuilder endpoints, IConfiguration configuration, IWebHostEnvironment env)
    {
        var graphQlOptions = configuration.GetOptions<GraphQlOptions>(OptionsSectionName);
        
        return endpoints
            .MapGraphQL(graphQlOptions.Endpoint)
            .WithOptions(new GraphQLServerOptions
            {
                Tool =
                {
                    Enable = env.IsDevelopment(),
                    Title = "AZ Integrator Portal GraphQL"
                }
            });
    }
}