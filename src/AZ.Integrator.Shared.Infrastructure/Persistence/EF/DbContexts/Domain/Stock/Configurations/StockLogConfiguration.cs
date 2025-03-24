using AZ.Integrator.Stocks.Domain.Aggregates;
using AZ.Integrator.Stocks.Domain.Aggregates.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Domain.Stock.Configurations;

public class StockLogConfiguration : IEntityTypeConfiguration<StockLog>
{
    public void Configure(EntityTypeBuilder<StockLog> builder)
    {
        builder.ToTable("stock_logs");

        builder.Ignore(e => e.Events);
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, id => new StockLogId(id))
            .UseIdentityColumn();
        
        builder.Property<PackageCode>("_packageCode")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("package_code")
            .IsRequired();
        
        builder.Property(e => e.ChangeQuantity)
            .HasConversion(value => value.Value, value => new ChangeQuantity(value))
            .HasColumnName("change_quantity")
            .IsRequired();
        
        builder.OwnsOne(e => e.CreationInformation, ci =>
        {
            ci.Property(c => c.CreatedAt).HasColumnName("created_at").IsRequired();
            ci.Property(c => c.OperatorLogin).HasColumnName("created_by").IsRequired();
            ci.Property(c => c.OperatorId).HasColumnName("created_by_id").IsRequired();
        });

        builder.Navigation(e => e.CreationInformation).IsRequired();
    }
}