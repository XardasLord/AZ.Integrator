using AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.View.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View;

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