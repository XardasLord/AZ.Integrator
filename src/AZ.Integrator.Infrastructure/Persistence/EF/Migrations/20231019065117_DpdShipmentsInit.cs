using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AZ.Integrator.Infrastructure.Persistence.EF.Migrations
{
    /// <inheritdoc />
    public partial class DpdShipmentsInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dpd_shipments",
                columns: table => new
                {
                    session_id = table.Column<long>(type: "bigint", nullable: false),
                    allegro_order_number = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dpd_shipments", x => x.session_id);
                });

            migrationBuilder.CreateTable(
                name: "dpd_packages",
                columns: table => new
                {
                    number = table.Column<long>(type: "bigint", nullable: false),
                    shipment_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dpd_packages", x => x.number);
                    table.ForeignKey(
                        name: "FK_dpd_packages_dpd_shipments_shipment_id",
                        column: x => x.shipment_id,
                        principalTable: "dpd_shipments",
                        principalColumn: "session_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dpd_parcels",
                columns: table => new
                {
                    number = table.Column<long>(type: "bigint", nullable: false),
                    waybill = table.Column<string>(type: "text", nullable: true),
                    package_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dpd_parcels", x => x.number);
                    table.ForeignKey(
                        name: "FK_dpd_parcels_dpd_packages_package_id",
                        column: x => x.package_id,
                        principalTable: "dpd_packages",
                        principalColumn: "number",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dpd_packages_shipment_id",
                table: "dpd_packages",
                column: "shipment_id");

            migrationBuilder.CreateIndex(
                name: "IX_dpd_parcels_package_id",
                table: "dpd_parcels",
                column: "package_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dpd_parcels");

            migrationBuilder.DropTable(
                name: "dpd_packages");

            migrationBuilder.DropTable(
                name: "dpd_shipments");
        }
    }
}
