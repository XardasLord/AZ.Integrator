using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ParcelTemplate.Configurations;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ParcelTemplate;

public class TagParcelTemplateDataViewContext : DbContext
{
    public virtual DbSet<TagParcelTemplateViewModel> TagParcelTemplates { get; set; }
    
    public TagParcelTemplateDataViewContext(DbContextOptions<TagParcelTemplateDataViewContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TagParcelTemplateViewModelConfiguration());
        modelBuilder.ApplyConfiguration(new TagParcelViewModelConfiguration());
    }
}