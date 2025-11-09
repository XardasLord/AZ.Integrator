using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.ValueObjects;
using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AZ.Integrator.Catalog.Infrastructure.Persistence.EF.Domain.Configurations;

public class PartDefinitionConfiguration : IEntityTypeConfiguration<PartDefinition>
{
    public void Configure(EntityTypeBuilder<PartDefinition> builder)
    {
        builder.ToTable("part_definitions", SchemaDefinition.Catalog);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasConversion(name => name.Value, name => new PartName(name))
            .IsRequired();

        builder.Property(e => e.Quantity)
            .HasColumnName("quantity")
            .HasConversion(quantity => quantity.Value, quantity => new Quantity(quantity))
            .IsRequired();

        builder.Property(e => e.AdditionalInfo)
            .HasColumnName("additional_info")
            .IsRequired(false);

        builder.OwnsOne(e => e.Dimensions, dimensions =>
        {
            dimensions.Property(d => d.LengthMm)
                .HasColumnName("length_mm")
                .IsRequired();

            dimensions.Property(d => d.WidthMm)
                .HasColumnName("width_mm")
                .IsRequired();

            dimensions.Property(d => d.ThicknessMm)
                .HasColumnName("thickness_mm")
                .IsRequired();

            dimensions.Property(d => d.LengthEdgeBandingType)
                .HasColumnName("edge_band_length_sides")
                .HasConversion(new EnumToNumberConverter<EdgeBandingType, int>())
                .IsRequired();

            dimensions.Property(d => d.WidthEdgeBandingType)
                .HasColumnName("edge_band_width_sides")
                .HasConversion(new EnumToNumberConverter<EdgeBandingType, int>())
                .IsRequired();
        });

        builder.Navigation(e => e.Dimensions).IsRequired();

        // Foreign key properties for composite key relationship
        builder.Property<FurnitureCode>("FurnitureCode")
            .HasColumnName("furniture_code")
            .HasConversion(code => code.Value, code => new FurnitureCode(code))
            .IsRequired();

        builder.Property<TenantId>("TenantId")
            .HasColumnName("tenant_id")
            .HasConversion(id => id.Value, id => new TenantId(id))
            .IsRequired();
    }
}
