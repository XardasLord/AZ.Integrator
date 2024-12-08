#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Domain.Shipment.Migrations
{
    /// <inheritdoc />
    public partial class AltertInpostShipmentAddTrackingNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "tracking_number",
                table: "inpost_shipments",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tracking_number",
                table: "inpost_shipments");
        }
    }
}
