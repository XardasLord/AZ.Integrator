using AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.EF.View.Configurations;
using AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.EF.View.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.EF.View;

public class TagParcelTemplateDataViewContext(DbContextOptions<TagParcelTemplateDataViewContext> options)
    : DbContext(options)
{
    public virtual DbSet<TagParcelTemplateViewModel> TagParcelTemplates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TagParcelTemplateViewModelConfiguration());
        modelBuilder.ApplyConfiguration(new TagParcelViewModelConfiguration());
    }
}