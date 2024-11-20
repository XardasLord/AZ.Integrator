#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Domain.Shipment.Migrations
{
    /// <inheritdoc />
    public partial class AdjustInpostShipmentsAddParcels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tracking_number",
                table: "inpost_shipments");

            migrationBuilder.CreateTable(
                name: "inpost_parcels",
                columns: table => new
                {
                    tracking_number = table.Column<string>(type: "text", nullable: false),
                    shipment_number = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inpost_parcels", x => x.tracking_number);
                    table.ForeignKey(
                        name: "FK_inpost_parcels_inpost_shipments_shipment_number",
                        column: x => x.shipment_number,
                        principalTable: "inpost_shipments",
                        principalColumn: "number",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_inpost_parcels_shipment_number",
                table: "inpost_parcels",
                column: "shipment_number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inpost_parcels");

            migrationBuilder.AddColumn<string>(
                name: "tracking_number",
                table: "inpost_shipments",
                type: "text",
                nullable: true);
        }
    }
}
