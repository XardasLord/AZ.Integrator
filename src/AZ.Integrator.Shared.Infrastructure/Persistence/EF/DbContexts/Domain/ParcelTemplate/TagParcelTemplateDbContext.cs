using AZ.Integrator.Shared.Infrastructure.Mediator;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Domain.ParcelTemplate.Configurations;
using AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Domain.ParcelTemplate;

public class TagParcelTemplateDbContext : DbContext
{
    private readonly IMediator _mediator;
    
    public virtual DbSet<TagParcelTemplate> TagParcelTemplates { get; set; }
    
    public TagParcelTemplateDbContext(DbContextOptions<TagParcelTemplateDbContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TagParcelTemplateConfiguration());
        modelBuilder.ApplyConfiguration(new TagParcelConfiguration());
    }
    
    public async Task SaveAggregateAsync(CancellationToken cancellationToken)
    {
        await base.SaveChangesAsync(cancellationToken);
        await _mediator.DispatchDomainEventsAsync(this);
    }
}