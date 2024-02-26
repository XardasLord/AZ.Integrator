using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.Migrations
{
    /// <inheritdoc />
    public partial class AdjustShipmentsViewRemoveTrackingNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS inpost_shipments_view;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS dpd_shipments_view;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS shipments_view;");
            
            migrationBuilder.Sql($@"
CREATE VIEW dpd_shipments_view
AS
SELECT
  session_id,
  allegro_order_number,
  created_at
FROM dpd_shipments;
            ");
            
            
            migrationBuilder.Sql($@"
CREATE VIEW inpost_shipments_view
AS
SELECT
  number,
  allegro_order_number,
  created_at
FROM inpost_shipments;;
            ");
            
            
            migrationBuilder.Sql($@"
CREATE VIEW shipments_view
AS
SELECT
  number as shipment_number,
  allegro_order_number,
  'INPOST' as shipment_provider,
  created_at
FROM inpost_shipments

UNION ALL

SELECT
    session_id::text as shipment_number,
    allegro_order_number,
    'DPD' as shipment_provider,
    created_at
FROM dpd_shipments;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW inpost_shipments_view;");
            migrationBuilder.Sql("DROP VIEW dpd_shipments_view;");
            migrationBuilder.Sql("DROP VIEW shipments_view;");
        }
    }
}
