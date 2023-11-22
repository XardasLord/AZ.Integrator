using Hangfire.Dashboard;

namespace AZ.Integrator.Shared.Infrastructure.Hangfire;

public class DashboardNoAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext dashboardContext)
    {
        return true;
    }
}