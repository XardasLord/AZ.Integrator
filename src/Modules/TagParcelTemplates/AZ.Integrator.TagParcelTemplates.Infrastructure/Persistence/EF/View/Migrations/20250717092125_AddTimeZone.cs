using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.ParcelTemplate.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeZone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS tag_parcels_view;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS tag_parcel_templates_view;");
            
            migrationBuilder.Sql($@"
CREATE OR REPLACE VIEW tag_parcel_templates_view
AS
SELECT
  tag,
  tenant_id,
  created_at,
  created_by
FROM tag_parcel_templates;

CREATE OR REPLACE VIEW tag_parcels_view
AS
SELECT
  id,
  tag,
  tenant_id,
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
