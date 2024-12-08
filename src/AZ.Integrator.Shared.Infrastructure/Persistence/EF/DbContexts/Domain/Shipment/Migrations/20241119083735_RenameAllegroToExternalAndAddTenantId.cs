using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Domain.Shipment.Migrations
{
    /// <inheritdoc />
    public partial class RenameAllegroToExternalAndAddTenantId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "allegro_order_number",
                table: "inpost_shipments",
                newName: "external_order_number");

            migrationBuilder.RenameColumn(
                name: "allegro_order_number",
                table: "dpd_shipments",
                newName: "external_order_number");

            migrationBuilder.AddColumn<string>(
                name: "tenant_id",
                table: "inpost_shipments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "tenant_id",
                table: "dpd_shipments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tenant_id",
                table: "inpost_shipments");

            migrationBuilder.DropColumn(
                name: "tenant_id",
                table: "dpd_shipments");

            migrationBuilder.RenameColumn(
                name: "external_order_number",
                table: "inpost_shipments",
                newName: "allegro_order_number");

            migrationBuilder.RenameColumn(
                name: "external_order_number",
                table: "dpd_shipments",
                newName: "allegro_order_number");
        }
    }
}
