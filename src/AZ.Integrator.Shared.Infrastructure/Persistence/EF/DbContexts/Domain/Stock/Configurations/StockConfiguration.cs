using AZ.Integrator.Stocks.Domain.Aggregates.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Domain.Stock.Configurations;

public class StockConfiguration : IEntityTypeConfiguration<Stocks.Domain.Aggregates.Stock>
{
    public void Configure(EntityTypeBuilder<Stocks.Domain.Aggregates.Stock> builder)
    {
        builder.ToTable("stocks");

        builder.Ignore(e => e.Events);
        builder.HasKey(e => e.PackageCode);

        builder.Property(e => e.PackageCode)
            .HasColumnName("package_code")
            .HasConversion(code => code.Value, code => new PackageCode(code))
            .IsRequired();

        builder.Property(e => e.Quantity)
            .HasColumnName("quantity")
            .HasConversion(quantity => quantity.Value, quantity => new Quantity(quantity))
            .IsRequired();

        builder.HasMany(e => e.StockLogs)
            .WithOne()
            .HasForeignKey("_packageCode");
    }
}