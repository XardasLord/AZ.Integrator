using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tag = AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate.ValueObjects.Tag;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Domain.ParcelTemplate.Configurations;

public class TagParcelTemplateConfiguration : IEntityTypeConfiguration<TagParcelTemplate>
{
    public void Configure(EntityTypeBuilder<TagParcelTemplate> builder)
    {
        builder.ToTable("tag_parcel_templates");

        builder.Ignore(e => e.Events);
        builder.HasKey(e => new { e.Tag, e.TenantId });

        builder.Property(e => e.Tag)
            .HasColumnName("tag")
            .HasConversion(tag => tag.Value, tag => new Tag(tag))
            .IsRequired();

        builder.Property(e => e.TenantId)
            .HasColumnName("tenant_id")
            .HasConversion(tag => tag.Value, tag => new TenantId(tag))
            .IsRequired();
        
        builder.OwnsOne(e => e.CreationInformation, ci =>
        {
            ci.Property(c => c.CreatedAt).HasColumnName("created_at").IsRequired();
            ci.Property(c => c.CreatedBy).HasColumnName("created_by").IsRequired();
            ci.Ignore(c => c.TenantId); // This is for HasKey purposes, so TenantId is stored as a separate property on the top level
        });

        builder.Navigation(e => e.CreationInformation).IsRequired();

        builder.HasMany(e => e.Parcels)
            .WithOne()
            .HasForeignKey(x => new { x.Tag, x.TenantId });
    }
}