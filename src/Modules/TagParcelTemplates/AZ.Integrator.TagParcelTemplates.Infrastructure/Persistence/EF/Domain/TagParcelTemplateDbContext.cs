using AZ.Integrator.Shared.Infrastructure.Mediator;
using AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate;
using AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.EF.Domain.Configurations;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.EF.Domain;

public class TagParcelTemplateDbContext(DbContextOptions<TagParcelTemplateDbContext> options, IMediator mediator)
    : DbContext(options)
{
    public virtual DbSet<TagParcelTemplate> TagParcelTemplates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TagParcelTemplateConfiguration());
        modelBuilder.ApplyConfiguration(new TagParcelConfiguration());
    }
    
    public async Task SaveAggregateAsync(CancellationToken cancellationToken)
    {
        await base.SaveChangesAsync(cancellationToken);
        await mediator.DispatchDomainEventsAsync(this);
    }
}