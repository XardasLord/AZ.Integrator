using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.Migrations
{
    /// <inheritdoc />
    public partial class InitTagParcelTemplatesView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
CREATE VIEW tag_parcel_templates_view
AS
SELECT
  tag,
  tenant_id,
  created_at,
  created_by
FROM tag_parcel_templates;

CREATE VIEW tag_parcels_view
AS
SELECT
  id,
  tag,
  length,
  width,
  height,
  weight
FROM tag_parcels;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW tag_parcels_view;");
            migrationBuilder.Sql("DROP VIEW tag_parcel_templates_view;");
        }
    }
}
