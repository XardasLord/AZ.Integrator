using AZ.Integrator.Stocks.Domain.Aggregates.Stock;
using AZ.Integrator.Stocks.Domain.Aggregates.Stock.ValueObjects;
using AZ.Integrator.Stocks.Domain.Aggregates.StockGroup.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Stocks.Infrastructure.Persistence.EF.Domain.Configurations;

public class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.ToTable("stocks");

        builder.Ignore(e => e.Events);
        builder.HasKey(e => e.PackageCode);
        
        builder.Property<StockGroupId>("_groupId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("group_id")
            .HasConversion(id => id.Value, id => new StockGroupId(id))
            .IsRequired(false);

        builder.Property(e => e.PackageCode)
            .HasColumnName("package_code")
            .HasConversion(code => code.Value, code => new PackageCode(code))
            .IsRequired();

        builder.Property(e => e.Quantity)
            .HasColumnName("quantity")
            .HasConversion(quantity => quantity.Value, quantity => new Quantity(quantity))
            .IsRequired();

        builder.Property(e => e.Threshold)
            .HasColumnName("threshold")
            .HasConversion(quantity => quantity.Value, quantity => new Quantity(quantity))
            .IsRequired();

        builder.HasMany(e => e.StockLogs)
            .WithOne()
            .HasForeignKey("_packageCode");
    }
}