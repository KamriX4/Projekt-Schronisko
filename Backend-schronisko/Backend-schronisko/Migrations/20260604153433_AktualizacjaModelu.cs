using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_schronisko.Migrations
{
    /// <inheritdoc />
    public partial class AktualizacjaModelu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZadaniaHarmonogramu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tytul = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kategoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCzas = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CzyWykonane = table.Column<bool>(type: "bit", nullable: false),
                    ZwierzeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZadaniaHarmonogramu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZadaniaHarmonogramu_Zwierzeta_ZwierzeId",
                        column: x => x.ZwierzeId,
                        principalTable: "Zwierzeta",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ZadaniaHarmonogramu_ZwierzeId",
                table: "ZadaniaHarmonogramu",
                column: "ZwierzeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZadaniaHarmonogramu");
        }
    }
}
