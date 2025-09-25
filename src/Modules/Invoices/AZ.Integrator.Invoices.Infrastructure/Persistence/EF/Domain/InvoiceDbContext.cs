using AZ.Integrator.Invoices.Infrastructure.Persistence.EF.Domain.Configurations;
using AZ.Integrator.Shared.Infrastructure.Mediator;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Invoices.Infrastructure.Persistence.EF.Domain;

public class InvoiceDbContext : DbContext
{
    private readonly IMediator _mediator;
    
    public virtual DbSet<Invoices.Domain.Aggregates.Invoice.Invoice> Invoices { get; set; }
    
    public InvoiceDbContext(DbContextOptions<InvoiceDbContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
    }
    
    public async Task SaveAggregateAsync(CancellationToken cancellationToken)
    {
        await base.SaveChangesAsync(cancellationToken);
        await _mediator.DispatchDomainEventsAsync(this);
    }
}