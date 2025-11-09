using AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.EF.View.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.EF.View.Configurations;

public class TagParcelTemplateViewModelConfiguration : IEntityTypeConfiguration<TagParcelTemplateViewModel>
{
    public void Configure(EntityTypeBuilder<TagParcelTemplateViewModel> builder)
    {
        builder.ToView("tag_parcel_templates_view");
        builder.HasKey(x => x.Tag);

        builder.Property(x => x.Tag).HasColumnName("tag");
        
        builder.HasMany(x => x.Parcels)
            .WithOne()
            .HasForeignKey(x => x.Tag);
    }
}