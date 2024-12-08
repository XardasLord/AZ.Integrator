#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Shipment.Migrations
{
    /// <inheritdoc />
    public partial class AdjustShipmentsView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
CREATE VIEW shipments_view
AS
SELECT
  number as shipment_number,
  allegro_order_number,
  tracking_number,
  'INPOST' as shipment_provider,
  created_at
FROM inpost_shipments

UNION ALL

SELECT
    session_id::text as shipment_number,
    allegro_order_number,
    null::text AS tracking_number,
    'DPD' as shipment_provider,
    created_at
FROM dpd_shipments;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW dpd_shipments_view;");
            migrationBuilder.Sql("DROP VIEW shipments_view;");
        }
    }
}
