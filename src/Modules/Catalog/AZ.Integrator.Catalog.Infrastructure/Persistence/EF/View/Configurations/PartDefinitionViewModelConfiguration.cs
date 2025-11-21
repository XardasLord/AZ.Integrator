using AZ.Integrator.Catalog.Contracts.FurnitureModels;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AZ.Integrator.Catalog.Infrastructure.Persistence.EF.View.Configurations;

public class PartDefinitionViewModelConfiguration : IEntityTypeConfiguration<PartDefinitionViewModel>
{
    public void Configure(EntityTypeBuilder<PartDefinitionViewModel> builder)
    {
        builder.ToView("part_definition_view", SchemaDefinition.Catalog);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.FurnitureCode)
            .HasColumnName("furniture_code");
            
        builder.Property(x => x.TenantId)
            .HasColumnName("tenant_id");
            
        builder.Property(x => x.Name)
            .HasColumnName("name");
            
        builder.Property(x => x.Quantity)
            .HasColumnName("quantity");
            
        builder.Property(x => x.AdditionalInfo)
            .HasColumnName("additional_info");

        builder.OwnsOne(x => x.Dimensions, d =>
        {
            d.Property(p => p.LengthMm)
                .HasColumnName("length_mm");

            d.Property(p => p.WidthMm)
                .HasColumnName("width_mm");

            d.Property(p => p.ThicknessMm)
                .HasColumnName("thickness_mm");

            d.Property(p => p.LengthEdgeBandingType)
                .HasColumnName("edge_band_length_sides")
                .HasConversion(new EnumToNumberConverter<EdgeBandingTypeViewModel, int>());

            d.Property(p => p.WidthEdgeBandingType)
                .HasColumnName("edge_band_width_sides")
                .HasConversion(new EnumToNumberConverter<EdgeBandingTypeViewModel, int>());
        });
    }
}