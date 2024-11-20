#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace AZ.Integrator.Shared.Infrastructure.Persistence.EF.DbContexts.Domain.ParcelTemplate.Migrations
{
    /// <inheritdoc />
    public partial class InitTagParcelTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tag_parcel_templates",
                columns: table => new
                {
                    tag = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag_parcel_templates", x => x.tag);
                });

            migrationBuilder.CreateTable(
                name: "tag_parcels",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    length = table.Column<double>(type: "double precision", nullable: true),
                    width = table.Column<double>(type: "double precision", nullable: true),
                    height = table.Column<double>(type: "double precision", nullable: true),
                    weight = table.Column<double>(type: "double precision", nullable: false),
                    tag = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag_parcels", x => x.id);
                    table.ForeignKey(
                        name: "FK_tag_parcels_tag_parcel_templates_tag",
                        column: x => x.tag,
                        principalTable: "tag_parcel_templates",
                        principalColumn: "tag",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tag_parcels_tag",
                table: "tag_parcels",
                column: "tag");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tag_parcels");

            migrationBuilder.DropTable(
                name: "tag_parcel_templates");
        }
    }
}
