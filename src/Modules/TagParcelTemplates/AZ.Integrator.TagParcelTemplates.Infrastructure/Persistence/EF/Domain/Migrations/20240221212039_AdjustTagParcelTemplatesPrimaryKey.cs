#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Domain.ParcelTemplate.Migrations
{
    /// <inheritdoc />
    public partial class AdjustTagParcelTemplatesPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tag_parcels_tag_parcel_templates_tag",
                table: "tag_parcels");

            migrationBuilder.DropIndex(
                name: "IX_tag_parcels_tag",
                table: "tag_parcels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tag_parcel_templates",
                table: "tag_parcel_templates");

            migrationBuilder.AddColumn<string>(
                name: "tenant_id",
                table: "tag_parcels",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tag_parcel_templates",
                table: "tag_parcel_templates",
                columns: new[] { "tag", "tenant_id" });

            migrationBuilder.CreateIndex(
                name: "IX_tag_parcels_tag_tenant_id",
                table: "tag_parcels",
                columns: new[] { "tag", "tenant_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_tag_parcels_tag_parcel_templates_tag_tenant_id",
                table: "tag_parcels",
                columns: new[] { "tag", "tenant_id" },
                principalTable: "tag_parcel_templates",
                principalColumns: new[] { "tag", "tenant_id" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tag_parcels_tag_parcel_templates_tag_tenant_id",
                table: "tag_parcels");

            migrationBuilder.DropIndex(
                name: "IX_tag_parcels_tag_tenant_id",
                table: "tag_parcels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tag_parcel_templates",
                table: "tag_parcel_templates");

            migrationBuilder.DropColumn(
                name: "tenant_id",
                table: "tag_parcels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tag_parcel_templates",
                table: "tag_parcel_templates",
                column: "tag");

            migrationBuilder.CreateIndex(
                name: "IX_tag_parcels_tag",
                table: "tag_parcels",
                column: "tag");

            migrationBuilder.AddForeignKey(
                name: "FK_tag_parcels_tag_parcel_templates_tag",
                table: "tag_parcels",
                column: "tag",
                principalTable: "tag_parcel_templates",
                principalColumn: "tag",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
