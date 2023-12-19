using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tag = AZ.Integrator.TagParcelTemplates.Domain.Aggregates.TagParcelTemplate.ValueObjects.Tag;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.Configurations.Domain;

public class TagParcelTemplateConfiguration : IEntityTypeConfiguration<TagParcelTemplate>
{
    public void Configure(EntityTypeBuilder<TagParcelTemplate> builder)
    {
        builder.ToTable("tag_parcel_templates");

        builder.Ignore(e => e.Events);
        builder.HasKey(e => e.Tag);

        builder.Property(e => e.Tag)
            .HasColumnName("tag")
            .HasConversion(tag => tag.Value, tag => new Tag(tag));
        
        builder.OwnsOne(e => e.CreationInformation, ci =>
        {
            ci.Property(c => c.TenantId)
                .HasColumnName("tenant_id")
                .HasConversion(id => id.Value, id => new TenantId(id))
                .IsRequired();
            
            ci.Property(c => c.CreatedAt).HasColumnName("created_at").IsRequired();
            ci.Property(c => c.CreatedBy).HasColumnName("created_by").IsRequired();
        });

        builder.Navigation(e => e.CreationInformation).IsRequired();

        builder.HasMany(e => e.Parcels)
            .WithOne()
            .HasForeignKey("_tag");
    }
}