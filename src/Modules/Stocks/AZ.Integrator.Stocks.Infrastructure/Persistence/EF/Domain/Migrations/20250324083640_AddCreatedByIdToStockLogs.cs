#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace AZ.Integrator.Stocks.Infrastructure.Persistence.EF.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByIdToStockLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "created_by_id",
                table: "stock_logs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_by_id",
                table: "stock_logs");
        }
    }
}
