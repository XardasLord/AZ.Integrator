using AZ.Integrator.Shared.Infrastructure.Mediator;
using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment;
using AZ.Integrator.Shipments.Infrastructure.Persistence.EF.Domain.Configurations;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Shipments.Infrastructure.Persistence.EF.Domain;

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
        modelBuilder.ApplyConfiguration(new InpostParcelConfiguration());
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