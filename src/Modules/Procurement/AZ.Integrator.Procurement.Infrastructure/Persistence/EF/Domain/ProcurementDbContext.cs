using AZ.Integrator.Procurement.Domain.Aggregates.Supplier;
using AZ.Integrator.Procurement.Infrastructure.Persistence.EF.Domain.Configurations;
using AZ.Integrator.Shared.Infrastructure.Mediator;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Procurement.Infrastructure.Persistence.EF.Domain;

public class ProcurementDbContext(DbContextOptions<ProcurementDbContext> options, IMediator mediator) 
    : DbContext(options)
{
    public DbSet<Supplier> Suppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SupplierConfiguration());
        modelBuilder.ApplyConfiguration(new SupplierMailboxConfiguration());

        base.OnModelCreating(modelBuilder);
    }
    
    public async Task SaveAggregateAsync(CancellationToken cancellationToken)
    {
        await base.SaveChangesAsync(cancellationToken);
        await mediator.DispatchDomainEventsAsync(this);
    }
}
