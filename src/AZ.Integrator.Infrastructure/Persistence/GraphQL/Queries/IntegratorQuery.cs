using AZ.Integrator.Domain.Abstractions;

namespace AZ.Integrator.Infrastructure.Persistence.GraphQL.Queries;

[ExtendObjectType(Name = "IntegratorQuery")]
internal class IntegratorQuery
{
    private readonly ICurrentUser _currentUser;
    
    public IntegratorQuery(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public string GetTest()
    {
        return "Test";
    }
}