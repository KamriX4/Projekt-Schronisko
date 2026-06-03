using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_schronisko.Migrations
{
    /// <inheritdoc />
    public partial class ZmianaNaGosciaIPracownikow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Uzytkownicy",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Haslo", "Login", "Rola" },
                values: new object[] { "gosc123", "gosc", "gosc" });

            migrationBuilder.InsertData(
                table: "Uzytkownicy",
                columns: new[] { "Id", "Haslo", "Login", "Rola" },
                values: new object[] { 3, "haslo123", "pracownik2", "pracownik" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Uzytkownicy",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Uzytkownicy",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Haslo", "Login", "Rola" },
                values: new object[] { "admin123", "admin", "admin" });
        }
    }
}
