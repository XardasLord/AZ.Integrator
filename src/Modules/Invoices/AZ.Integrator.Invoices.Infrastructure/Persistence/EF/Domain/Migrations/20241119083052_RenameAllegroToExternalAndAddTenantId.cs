using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Domain.Invoice.Migrations
{
    /// <inheritdoc />
    public partial class RenameAllegroToExternalAndAddTenantId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "allegro_order_number",
                table: "invoices",
                newName: "external_order_number");

            migrationBuilder.AddColumn<string>(
                name: "tenant_id",
                table: "invoices",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tenant_id",
                table: "invoices");

            migrationBuilder.RenameColumn(
                name: "external_order_number",
                table: "invoices",
                newName: "allegro_order_number");
        }
    }
}
