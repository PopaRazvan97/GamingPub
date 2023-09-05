using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class MadeAddressIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamingPubs_Adress_AdressId",
                table: "GamingPubs");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Adress_AdressId",
                table: "User");

            migrationBuilder.DropTable(
                name: "Adress");

            migrationBuilder.DropIndex(
                name: "IX_User_AdressId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_GamingPubs_AdressId",
                table: "GamingPubs");

            migrationBuilder.DropColumn(
                name: "AdressId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "AdressId",
                table: "GamingPubs");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "GamingPubs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_AddressId",
                table: "User",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_GamingPubs_AddressId",
                table: "GamingPubs",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_GamingPubs_Address_AddressId",
                table: "GamingPubs",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Address_AddressId",
                table: "User",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamingPubs_Address_AddressId",
                table: "GamingPubs");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Address_AddressId",
                table: "User");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropIndex(
                name: "IX_User_AddressId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_GamingPubs_AddressId",
                table: "GamingPubs");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "GamingPubs");

            migrationBuilder.AddColumn<int>(
                name: "AdressId",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AdressId",
                table: "GamingPubs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Adress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adress", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_AdressId",
                table: "User",
                column: "AdressId");

            migrationBuilder.CreateIndex(
                name: "IX_GamingPubs_AdressId",
                table: "GamingPubs",
                column: "AdressId");

            migrationBuilder.AddForeignKey(
                name: "FK_GamingPubs_Adress_AdressId",
                table: "GamingPubs",
                column: "AdressId",
                principalTable: "Adress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Adress_AdressId",
                table: "User",
                column: "AdressId",
                principalTable: "Adress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
