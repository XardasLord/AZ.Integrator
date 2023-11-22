namespace AZ.Integrator.Shared.Infrastructure.Persistence.GraphQL;

public class GraphQlOptions
{
    public string Endpoint { get; set; }
    public int MaxPageSize { get; set; }
    public int DefaultPageSize { get; set; }
}