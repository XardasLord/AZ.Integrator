using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AZ.Integrator.Infrastructure.Persistence.EF.Migrations
{
    /// <inheritdoc />
    public partial class InvoicesViewInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
CREATE VIEW invoices_view
AS
SELECT
  number,
  allegro_order_number,
  created_at
FROM invoices;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW inpost_shipments_view;");
        }
    }
}
