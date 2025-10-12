using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel;
using AZ.Integrator.Catalog.Domain.Aggregates.FurnitureModel.ValueObjects;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

        builder.Property(e => e.Color)
            .HasColumnName("color")
            .HasConversion(color => color.Value, color => new Color(color))
            .IsRequired(false);

        builder.Property(e => e.AdditionalInfo)
            .HasColumnName("additional_info")
            .IsRequired(false);

        // Configure Dimensions as owned entity
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
        });

        builder.Navigation(e => e.Dimensions).IsRequired();

        // Foreign key properties for composite key relationship
        builder.Property<string>("FurnitureCode")
            .HasColumnName("furniture_code")
            .IsRequired();

        builder.Property<Guid>("TenantId")
            .HasColumnName("tenant_id")
            .IsRequired();
    }
}
