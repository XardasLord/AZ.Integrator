using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Invoice.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeZone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS invoices_view;");
            
            migrationBuilder.Sql($@"
CREATE OR REPLACE VIEW invoices_view
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
