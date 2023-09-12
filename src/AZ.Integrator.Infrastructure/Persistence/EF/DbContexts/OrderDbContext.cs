using AZ.Integrator.Domain.Aggregates.Order;
using AZ.Integrator.Infrastructure.Mediator;
using AZ.Integrator.Infrastructure.Persistence.EF.Configurations.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Infrastructure.Persistence.EF.DbContexts;

public class OrderDbContext : DbContext
{
    private readonly IMediator _mediator;
    
    public virtual DbSet<Order> Orders { get; set; }
    
    public OrderDbContext(DbContextOptions<OrderDbContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
    }
    
    public async Task SaveAggregateAsync(CancellationToken cancellationToken)
    {
        await base.SaveChangesAsync(cancellationToken);
        await _mediator.DispatchDomainEventsAsync(this);
    }
}