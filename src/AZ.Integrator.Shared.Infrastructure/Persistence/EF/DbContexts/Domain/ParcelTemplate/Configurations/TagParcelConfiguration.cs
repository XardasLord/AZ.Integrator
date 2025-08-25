using AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate;
using AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tag = AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate.ValueObjects.Tag;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Domain.ParcelTemplate.Configurations;

public class TagParcelConfiguration : IEntityTypeConfiguration<TagParcel>
{
    public void Configure(EntityTypeBuilder<TagParcel> builder)
    {
        builder.ToTable("tag_parcels");

        builder.Ignore(e => e.Events);
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, id => new TagParcelId(id))
            .UseIdentityColumn();
        
        builder.Property(e => e.Tag)
            .HasConversion(id => id.Value, id => new Tag(id))
            .HasColumnName("tag")
            .IsRequired();

        builder.Property(e => e.Weight).HasColumnName("weight").IsRequired();
        
        builder.OwnsOne(e => e.Dimension, d =>
        {
            d.Property(c => c.Length).HasColumnName("length").IsRequired();
            d.Property(c => c.Width).HasColumnName("width").IsRequired();
            d.Property(c => c.Height).HasColumnName("height").IsRequired();
        });
    }
}