using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.Migrations
{
    /// <inheritdoc />
    public partial class AdjustAllegroAccountsAddClientOAuthRelatedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "client_id",
                table: "allegro_accounts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "client_secret",
                table: "allegro_accounts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "expires_at",
                table: "allegro_accounts",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "redirect_uri",
                table: "allegro_accounts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "client_id",
                table: "allegro_accounts");

            migrationBuilder.DropColumn(
                name: "client_secret",
                table: "allegro_accounts");

            migrationBuilder.DropColumn(
                name: "expires_at",
                table: "allegro_accounts");

            migrationBuilder.DropColumn(
                name: "redirect_uri",
                table: "allegro_accounts");
        }
    }
}
