using AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Stock.Configurations;

public class StockLogViewModelConfiguration : IEntityTypeConfiguration<StockLogViewModel>
{
    public void Configure(EntityTypeBuilder<StockLogViewModel> builder)
    {
        builder.ToView("stock_logs_view");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.PackageCode).HasColumnName("package_code");
        builder.Property(x => x.ChangeQuantity).HasColumnName("change_quantity");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.CreatedBy).HasColumnName("created_by");
    }
}