using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder;
using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.ValueObjects;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AZ.Integrator.Procurement.Infrastructure.Persistence.EF.Domain.Configurations;

public class OrderFurniturePartLineConfigurationConfiguration : IEntityTypeConfiguration<OrderFurniturePartLine>
{
    public void Configure(EntityTypeBuilder<OrderFurniturePartLine> builder)
    {
        builder.ToTable("order_furniture_part_lines", SchemaDefinition.Procurement);

        builder.Ignore(e => e.Events);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, id => new OrderFurniturePartLineId(id))
            .UseIdentityColumn();
        
        builder.Property<OrderFurnitureLineId>("_orderFurnitureLineId")
            .HasConversion(id => id.Value, id => new OrderFurnitureLineId(id))
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("order_furniture_line_id")
            .IsRequired();

        builder.Property(e => e.PartName)
            .HasColumnName("part_name")
            .HasConversion(name => name.Value, name => new PartName(name))
            .IsRequired();

        builder.Property(e => e.Quantity)
            .HasColumnName("quantity")
            .HasConversion(value => value.Value, value => new Quantity(value))
            .IsRequired();

        builder.Property(e => e.AdditionalInfo)
            .HasColumnName("additional_info")
            .IsRequired();
        
        builder.ComplexProperty(e => e.Dimensions, dimensions =>
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

            dimensions.IsRequired();
        });
    }
}
