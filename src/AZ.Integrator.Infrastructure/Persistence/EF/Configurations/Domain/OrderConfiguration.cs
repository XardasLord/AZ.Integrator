using AZ.Integrator.Domain.Aggregates.Order;
using AZ.Integrator.Domain.Aggregates.Order.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AZ.Integrator.Infrastructure.Persistence.EF.Configurations.Domain;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(e => e.Number);

        builder.Property(e => e.Number)
            .HasColumnName("number")
            .HasConversion(number => number.Value, number => new OrderNumber(number));
        
        builder.Property(b => b.Status)
            .HasColumnName("status")
            .HasConversion(new EnumToNumberConverter<OrderStatus, int>())
            .IsRequired();
    }
}