using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.View.Stock.Migrations
{
    /// <inheritdoc />
    public partial class InitStocksView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
CREATE VIEW stocks_view
AS
SELECT
  package_code,
  quantity
FROM stocks;

CREATE VIEW stock_logs_view
AS
SELECT
  id,
  change_quantity,
  created_at,
  created_by,
  package_code
FROM stock_logs;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW stock_logs_view;");
            migrationBuilder.Sql("DROP VIEW stocks_view;");
        }
    }
}
