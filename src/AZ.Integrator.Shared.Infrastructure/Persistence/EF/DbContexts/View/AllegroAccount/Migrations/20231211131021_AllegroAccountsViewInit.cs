#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.AllegroAccount.Migrations
{
    /// <inheritdoc />
    public partial class AllegroAccountsViewInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
CREATE VIEW allegro_accounts_view
AS
SELECT
  tenant_id,
  access_token,
  refresh_token
FROM allegro_accounts;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW allegro_accounts_view;");
        }
    }
}
