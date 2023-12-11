using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.Migrations
{
    /// <inheritdoc />
    public partial class AlterInvoiceDataViewAddInvoiceId : Migration
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
  allegro_order_number,
  created_at
FROM invoices;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW invoices_view;");
        }
    }
}
