using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AZ.Integrator.Infrastructure.Persistence.EF.Migrations
{
    /// <inheritdoc />
    public partial class AlterInpostShipmentsViewAddTrackingNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS inpost_shipments_view;");
            
            migrationBuilder.Sql($@"
CREATE VIEW inpost_shipments_view
AS
SELECT
  number,
  allegro_order_number,
  tracking_number,
  created_at
FROM inpost_shipments;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW inpost_shipments_view;");
        }
    }
}
