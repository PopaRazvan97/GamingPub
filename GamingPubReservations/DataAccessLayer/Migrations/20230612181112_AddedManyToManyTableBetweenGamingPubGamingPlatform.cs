using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddedManyToManyTableBetweenGamingPubGamingPlatform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GamingPubGamingPlatforms",
                columns: table => new
                {
                    GamingPubId = table.Column<int>(type: "int", nullable: false),
                    GamingPlatformId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamingPubGamingPlatforms", x => new { x.GamingPubId, x.GamingPlatformId });
                    table.ForeignKey(
                        name: "FK_GamingPubGamingPlatforms_GamingPlatforms_GamingPlatformId",
                        column: x => x.GamingPlatformId,
                        principalTable: "GamingPlatforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamingPubGamingPlatforms_GamingPubs_GamingPubId",
                        column: x => x.GamingPubId,
                        principalTable: "GamingPubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamingPubGamingPlatforms_GamingPlatformId",
                table: "GamingPubGamingPlatforms",
                column: "GamingPlatformId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamingPubGamingPlatforms");
        }
    }
}
