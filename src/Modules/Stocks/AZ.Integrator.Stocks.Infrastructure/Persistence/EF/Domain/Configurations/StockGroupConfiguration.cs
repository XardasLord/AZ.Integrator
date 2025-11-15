using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Stocks.Domain.Aggregates.StockGroup;
using AZ.Integrator.Stocks.Domain.Aggregates.StockGroup.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Stocks.Infrastructure.Persistence.EF.Domain.Configurations;

public class StockGroupConfiguration : IEntityTypeConfiguration<StockGroup>
{
    public void Configure(EntityTypeBuilder<StockGroup> builder)
    {
        builder.ToTable("stock_groups");

        builder.Ignore(e => e.Events);
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, id => new StockGroupId(id))
            .UseIdentityColumn();
        
        builder.Property(e => e.TenantId)
            .HasConversion(value => value.Value, value => new TenantId(value))
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasConversion(name => name.Value, name => new StockGroupName(name))
            .IsRequired();

        builder.Property(e => e.Description)
            .HasColumnName("description")
            .HasConversion(description => description.Value, description => new Description(description))
            .IsRequired();
        
        builder.OwnsOne(e => e.CreationInformation, ci =>
        {
            ci.Property(c => c.CreatedAt).HasColumnName("created_at").IsRequired();
            ci.Property(c => c.OperatorLogin).HasColumnName("created_by").IsRequired();
            ci.Property(c => c.OperatorId).HasColumnName("created_by_id").IsRequired();
        });
        
        builder.OwnsOne(e => e.ModificationInformation, mi =>
        {
            mi.Property(m => m.ModifiedAt).HasColumnName("modified_at");
            mi.Property(m => m.ModifiedBy).HasColumnName("modified_by");
        });

        builder.Navigation(e => e.CreationInformation).IsRequired();
    }
}