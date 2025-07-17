using AZ.Integrator.Stocks.Infrastructure.Persistence.EF.View.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Stocks.Infrastructure.Persistence.EF.View.Configurations;

public class StockViewModelConfiguration : IEntityTypeConfiguration<StockViewModel>
{
    public void Configure(EntityTypeBuilder<StockViewModel> builder)
    {
        builder.ToView("stocks_view");
        builder.HasKey(x => x.PackageCode);

        builder.Property(x => x.GroupId).HasColumnName("group_id");
        builder.Property(x => x.PackageCode).HasColumnName("package_code");
        builder.Property(x => x.Quantity).HasColumnName("quantity");
        
        builder.HasMany(x => x.Logs)
            .WithOne()
            .HasForeignKey(x => x.PackageCode);
    }
}