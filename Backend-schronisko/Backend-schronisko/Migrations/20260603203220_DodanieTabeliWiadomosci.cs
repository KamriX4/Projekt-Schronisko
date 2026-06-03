using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_schronisko.Migrations
{
    /// <inheritdoc />
    public partial class DodanieTabeliWiadomosci : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wiadomosci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tresc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataDodania = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nadawca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Odbiorca = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wiadomosci", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wiadomosci");
        }
    }
}
