using AZ.Integrator.Procurement.Contracts.PartDefinitionOrders;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AZ.Integrator.Procurement.Infrastructure.Persistence.EF.View.Configurations;

public class OrderFurniturePartLineViewModelConfiguration : IEntityTypeConfiguration<OrderFurniturePartLineViewModel>
{
    public void Configure(EntityTypeBuilder<OrderFurniturePartLineViewModel> builder)
    {
        builder.ToView("order_furniture_part_lines_view", SchemaDefinition.Procurement);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");
            
        builder.Property(x => x.OrderFurnitureLineId)
            .HasColumnName("order_furniture_line_id");
            
        builder.Property(x => x.Name)
            .HasColumnName("part_name");
            
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
                .HasConversion(new EnumToNumberConverter<OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel, int>());

            d.Property(p => p.WidthEdgeBandingType)
                .HasColumnName("edge_band_width_sides")
                .HasConversion(new EnumToNumberConverter<OrderFurniturePartLineDimensionsEdgeBandingTypeViewModel, int>());
        });
    }
}