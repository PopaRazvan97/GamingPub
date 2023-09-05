using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedScheduleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamingPubs_Schedules_ScheduleId",
                table: "GamingPubs");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_GamingPubs_ScheduleId",
                table: "GamingPubs");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "GamingPubs");

            migrationBuilder.CreateTable(
                name: "DaySchedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecialDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaySchedule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DayScheduleGamingPub",
                columns: table => new
                {
                    GamingPubsId = table.Column<int>(type: "int", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayScheduleGamingPub", x => new { x.GamingPubsId, x.ScheduleId });
                    table.ForeignKey(
                        name: "FK_DayScheduleGamingPub_DaySchedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "DaySchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DayScheduleGamingPub_GamingPubs_GamingPubsId",
                        column: x => x.GamingPubsId,
                        principalTable: "GamingPubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DayScheduleGamingPub_ScheduleId",
                table: "DayScheduleGamingPub",
                column: "ScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayScheduleGamingPub");

            migrationBuilder.DropTable(
                name: "DaySchedule");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "GamingPubs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Friday = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Monday = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Saturday = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sunday = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Thursday = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tuesday = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Wednesday = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamingPubs_ScheduleId",
                table: "GamingPubs",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_GamingPubs_Schedules_ScheduleId",
                table: "GamingPubs",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
