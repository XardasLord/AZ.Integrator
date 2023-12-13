using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.Migrations
{
    /// <inheritdoc />
    public partial class AdjustAllegroAccountsViewAddClientOAuthRelatedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS allegro_accounts_view;");
            
            migrationBuilder.Sql($@"
CREATE VIEW allegro_accounts_view
AS
SELECT
  tenant_id,
  access_token,
  refresh_token,
  expires_at,
  client_id,
  client_secret,
  redirect_uri
FROM allegro_accounts;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW allegro_accounts_view;");
        }
    }
}
