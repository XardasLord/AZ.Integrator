using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AZ.Integrator.Stocks.Infrastructure.Persistence.EF.View.Migrations
{
    /// <inheritdoc />
    public partial class AddThreshold : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
CREATE OR REPLACE VIEW stock_groups_view
AS
SELECT
  id,
  name,
  description,
  created_at,
  created_by,
  modified_at,
  modified_by
FROM stock_groups;

CREATE OR REPLACE VIEW stocks_view
AS
SELECT
  package_code,
  quantity,
  group_id,
  threshold
FROM stocks;

CREATE OR REPLACE VIEW stock_logs_view
AS
SELECT
  id,
  change_quantity,
  created_at,
  created_by,
  package_code,
  status
FROM stock_logs;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW stock_logs_view;");
            migrationBuilder.Sql("DROP VIEW stocks_view;");
            migrationBuilder.Sql("DROP VIEW stock_groups_view;");
        }
    }
}
