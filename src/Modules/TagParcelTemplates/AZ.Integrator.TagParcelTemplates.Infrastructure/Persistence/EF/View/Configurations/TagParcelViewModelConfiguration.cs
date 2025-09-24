using AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.EF.View.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.TagParcelTemplates.Infrastructure.Persistence.EF.View.Configurations;

public class TagParcelViewModelConfiguration : IEntityTypeConfiguration<TagParcelViewModel>
{
    public void Configure(EntityTypeBuilder<TagParcelViewModel> builder)
    {
        builder.ToView("tag_parcels_view");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Tag).HasColumnName("tag");
        builder.Property(x => x.Length).HasColumnName("length");
        builder.Property(x => x.Width).HasColumnName("width");
        builder.Property(x => x.Height).HasColumnName("height");
        builder.Property(x => x.Weight).HasColumnName("weight");
    }
}