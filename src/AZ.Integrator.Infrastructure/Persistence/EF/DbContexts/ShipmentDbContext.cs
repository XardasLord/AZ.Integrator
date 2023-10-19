using AZ.Integrator.Domain.Aggregates.DpdShipment;
using AZ.Integrator.Domain.Aggregates.InpostShipment;
using AZ.Integrator.Infrastructure.Mediator;
using AZ.Integrator.Infrastructure.Persistence.EF.Configurations.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Infrastructure.Persistence.EF.DbContexts;

public class ShipmentDbContext : DbContext
{
    private readonly IMediator _mediator;
    
    public virtual DbSet<InpostShipment> InpostShipments { get; set; }
    
    public virtual DbSet<DpdShipment> DpdShipments { get; set; }
    
    public ShipmentDbContext(DbContextOptions<ShipmentDbContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new InpostShipmentConfiguration());
        modelBuilder.ApplyConfiguration(new DpdShipmentConfiguration());
        modelBuilder.ApplyConfiguration(new DpdPackageConfiguration());
        modelBuilder.ApplyConfiguration(new DpdParcelConfiguration());
    }
    
    public async Task SaveAggregateAsync(CancellationToken cancellationToken)
    {
        await base.SaveChangesAsync(cancellationToken);
        await _mediator.DispatchDomainEventsAsync(this);
    }
}