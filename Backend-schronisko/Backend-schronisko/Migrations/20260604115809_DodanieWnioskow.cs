using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_schronisko.Migrations
{
    /// <inheritdoc />
    public partial class DodanieWnioskow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WnioskiAdopcyjne",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImieINazwisko = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZwierzeId = table.Column<int>(type: "int", nullable: false),
                    Uzasadnienie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataZlozenia = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WnioskiAdopcyjne", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WnioskiAdopcyjne_Zwierzeta_ZwierzeId",
                        column: x => x.ZwierzeId,
                        principalTable: "Zwierzeta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WnioskiOddania",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImieINazwiskoOddajacego = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImieZwierzaka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gatunek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PowodOddania = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataZlozenia = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WnioskiOddania", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WnioskiAdopcyjne_ZwierzeId",
                table: "WnioskiAdopcyjne",
                column: "ZwierzeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WnioskiAdopcyjne");

            migrationBuilder.DropTable(
                name: "WnioskiOddania");
        }
    }
}
