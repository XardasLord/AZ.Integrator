using AZ.Integrator.Stocks.Infrastructure.Persistence.EF.View.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Stocks.Infrastructure.Persistence.EF.View.Configurations;

public class StockGroupViewModelConfiguration : IEntityTypeConfiguration<StockGroupViewModel>
{
    public void Configure(EntityTypeBuilder<StockGroupViewModel> builder)
    {
        builder.ToView("stock_groups_view");
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Description).HasColumnName("description");
        
        builder.HasMany(x => x.Stocks)
            .WithOne()
            .HasForeignKey(x => x.GroupId);
    }
}