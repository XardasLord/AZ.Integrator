using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Infrastructure.ShopifyAccount.Migrations
{
    /// <inheritdoc />
    public partial class ShopifyAccountsInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "account");

            migrationBuilder.CreateTable(
                name: "shopify",
                schema: "account",
                columns: table => new
                {
                    tenant_id = table.Column<string>(type: "text", nullable: false),
                    api_url = table.Column<string>(type: "text", nullable: false),
                    admin_api_token = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shopify", x => x.tenant_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shopify",
                schema: "account");
        }
    }
}
