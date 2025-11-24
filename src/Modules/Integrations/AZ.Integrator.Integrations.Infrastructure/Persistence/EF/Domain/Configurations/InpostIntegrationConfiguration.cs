using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Integrations.Domain.Aggregates.Inpost;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Integrations.Infrastructure.Persistence.EF.Domain.Configurations;

public class InpostIntegrationConfiguration : IEntityTypeConfiguration<InpostIntegration>
{
    public void Configure(EntityTypeBuilder<InpostIntegration> builder)
    {
        builder.ToTable("inpost", SchemaDefinition.Integration);

        builder.HasKey(e => new { e.TenantId, e.OrganizationId });

        builder.Property(e => e.TenantId)
            .HasConversion(value => value.Value, value => new TenantId(value))
            .HasColumnName("tenant_id")
            .IsRequired();

        builder.Property(e => e.OrganizationId)
            .HasColumnName("organization_id")
            .IsRequired();

        builder.Property(e => e.AccessToken)
            .HasColumnName("access_token")
            .IsRequired();

        builder.Property(e => e.DisplayName)
            .HasColumnName("display_name")
            .IsRequired();

        builder.Property(e => e.IsEnabled)
            .HasColumnName("is_enabled")
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.ComplexProperty(e => e.SenderData, sd =>
        {
            sd.Property(x => x.Name).HasColumnName("sender_name").IsRequired();
            sd.Property(x => x.CompanyName).HasColumnName("sender_company_name").IsRequired();
            sd.Property(x => x.FirstName).HasColumnName("sender_first_name").IsRequired();
            sd.Property(x => x.LastName).HasColumnName("sender_last_name").IsRequired();
            sd.Property(x => x.Email).HasColumnName("sender_email").IsRequired();
            sd.Property(x => x.Phone).HasColumnName("sender_phone").IsRequired();

            sd.ComplexProperty(x => x.Address, ad =>
            {
                ad.Property(x => x.Street).HasColumnName("sender_address_street").IsRequired();
                ad.Property(x => x.BuildingNumber).HasColumnName("sender_address_building_number").IsRequired();
                ad.Property(x => x.City).HasColumnName("sender_address_city").IsRequired();
                ad.Property(x => x.PostCode).HasColumnName("sender_address_post_code").IsRequired();
                ad.Property(x => x.CountryCode).HasColumnName("sender_address_country_code").IsRequired();
                

                ad.IsRequired();
            });

            sd.IsRequired();
        });
    }
}