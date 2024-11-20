using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Shipment.Migrations
{
    /// <inheritdoc />
    public partial class RenameAllegroToExternalAndAddTenantId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS shipments_view;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS inpost_shipments_view;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS dpd_shipments_view;");
            
            migrationBuilder.Sql($@"
CREATE VIEW dpd_shipments_view
AS
SELECT
  session_id,
  external_order_number,
  created_at,
  tenant_id
FROM dpd_shipments;
            ");
            
            
            migrationBuilder.Sql($@"
CREATE VIEW inpost_shipments_view
AS
SELECT
  number,
  external_order_number,
  created_at,
  tenant_id
FROM inpost_shipments;;
            ");
            
            
            migrationBuilder.Sql($@"
CREATE VIEW shipments_view
AS
SELECT
  number as shipment_number,
  external_order_number,
  'INPOST' as shipment_provider,
  created_at,
  tenant_id
FROM inpost_shipments

UNION ALL

SELECT
    session_id::text as shipment_number,
    external_order_number,
    'DPD' as shipment_provider,
    created_at,
    tenant_id
FROM dpd_shipments;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW shipments_view;");
            migrationBuilder.Sql("DROP VIEW inpost_shipments_view;");
            migrationBuilder.Sql("DROP VIEW dpd_shipments_view;");
        }
    }
}
