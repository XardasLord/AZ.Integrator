using AZ.Integrator.Monitoring.Infrastructure.Persistence.EF.Domain.Configurations;
using AZ.Integrator.Shared.Infrastructure.Mediator;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Monitoring.Infrastructure.Persistence.EF.Domain;

public class MonitoringDbContext(DbContextOptions<MonitoringDbContext> options, IMediator mediator)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MonitoringConfiguration());
    }
    
    public async Task SaveAggregateAsync(CancellationToken cancellationToken)
    {
        await base.SaveChangesAsync(cancellationToken);
        await mediator.DispatchDomainEventsAsync(this);
    }
}