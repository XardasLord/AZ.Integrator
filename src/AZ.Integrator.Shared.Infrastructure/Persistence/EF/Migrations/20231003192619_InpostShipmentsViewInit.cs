using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AZ.Integrator.Infrastructure.Persistence.EF.Migrations
{
    /// <inheritdoc />
    public partial class InpostShipmentsViewInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
CREATE VIEW inpost_shipments_view
AS
SELECT
  number,
  allegro_order_number,
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
