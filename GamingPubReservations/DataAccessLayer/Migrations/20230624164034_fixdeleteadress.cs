using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class fixdeleteadress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamingPubs_Adress_AdressId",
                table: "GamingPubs");

            migrationBuilder.AlterColumn<int>(
                name: "AdressId",
                table: "GamingPubs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_GamingPubs_Adress_AdressId",
                table: "GamingPubs",
                column: "AdressId",
                principalTable: "Adress",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamingPubs_Adress_AdressId",
                table: "GamingPubs");

            migrationBuilder.AlterColumn<int>(
                name: "AdressId",
                table: "GamingPubs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GamingPubs_Adress_AdressId",
                table: "GamingPubs",
                column: "AdressId",
                principalTable: "Adress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
