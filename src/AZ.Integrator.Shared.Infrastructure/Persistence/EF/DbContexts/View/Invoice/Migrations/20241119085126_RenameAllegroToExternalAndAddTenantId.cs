using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Invoice.Migrations
{
    /// <inheritdoc />
    public partial class RenameAllegroToExternalAndAddTenantId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW invoices_view;");
            
            migrationBuilder.Sql($@"
CREATE VIEW invoices_view
AS
SELECT
  external_id,
  number,
  external_order_number,
  created_at,
  tenant_id
FROM invoices;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW invoices_view;");
        }
    }
}
