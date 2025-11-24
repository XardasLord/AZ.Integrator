using AZ.Integrator.Integrations.Contracts.ViewModels;
using AZ.Integrator.Shared.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AZ.Integrator.Integrations.Infrastructure.Persistence.EF.View.Configurations;

public class InpostIntegrationViewModelConfiguration : IEntityTypeConfiguration<InpostIntegrationViewModel>
{
    public void Configure(EntityTypeBuilder<InpostIntegrationViewModel> builder)
    {
        builder.ToView("inpost_view", SchemaDefinition.Integration);

        builder.HasKey(e => new { e.TenantId, e.OrganizationId });

        builder.Property(e => e.TenantId).HasColumnName("tenant_id");
        builder.Property(e => e.OrganizationId).HasColumnName("organization_id");
        builder.Property(e => e.AccessToken).HasColumnName("access_token");
        builder.Property(e => e.DisplayName).HasColumnName("display_name");
        builder.Property(e => e.IsEnabled).HasColumnName("is_enabled");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        
        builder.Property(e => e.SenderName).HasColumnName("sender_name");
        builder.Property(e => e.SenderCompanyName).HasColumnName("sender_company_name");
        builder.Property(e => e.SenderFirstName).HasColumnName("sender_first_name");
        builder.Property(e => e.SenderLastName).HasColumnName("sender_last_name");
        builder.Property(e => e.SenderEmail).HasColumnName("sender_email");
        builder.Property(e => e.SenderPhone).HasColumnName("sender_phone");
        builder.Property(e => e.SenderAddressStreet).HasColumnName("sender_address_street");
        builder.Property(e => e.SenderAddressBuildingNumber).HasColumnName("sender_address_building_number");
        builder.Property(e => e.SenderAddressCity).HasColumnName("sender_address_city");
        builder.Property(e => e.SenderAddressPostCode).HasColumnName("sender_address_post_code");
        builder.Property(e => e.SenderAddressCountryCode).HasColumnName("sender_address_country_code");
    }
}