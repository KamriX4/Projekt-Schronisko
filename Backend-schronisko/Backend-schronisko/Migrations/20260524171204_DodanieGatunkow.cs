using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend_schronisko.Migrations
{
    /// <inheritdoc />
    public partial class DodanieGatunkow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Gatunki",
                columns: new[] { "Id", "Nazwa" },
                values: new object[,]
                {
                    { 1, "Pies" },
                    { 2, "Kot" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Gatunki",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Gatunki",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
