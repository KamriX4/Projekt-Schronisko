using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_schronisko.Migrations
{
    /// <inheritdoc />
    public partial class DodanieKomentarzy1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Komentarz_Wiadomosci_WiadomoscId",
                table: "Komentarz");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Komentarz",
                table: "Komentarz");

            migrationBuilder.RenameTable(
                name: "Komentarz",
                newName: "Komentarze");

            migrationBuilder.RenameIndex(
                name: "IX_Komentarz_WiadomoscId",
                table: "Komentarze",
                newName: "IX_Komentarze_WiadomoscId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Komentarze",
                table: "Komentarze",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Komentarze_Wiadomosci_WiadomoscId",
                table: "Komentarze",
                column: "WiadomoscId",
                principalTable: "Wiadomosci",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Komentarze_Wiadomosci_WiadomoscId",
                table: "Komentarze");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Komentarze",
                table: "Komentarze");

            migrationBuilder.RenameTable(
                name: "Komentarze",
                newName: "Komentarz");

            migrationBuilder.RenameIndex(
                name: "IX_Komentarze_WiadomoscId",
                table: "Komentarz",
                newName: "IX_Komentarz_WiadomoscId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Komentarz",
                table: "Komentarz",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Komentarz_Wiadomosci_WiadomoscId",
                table: "Komentarz",
                column: "WiadomoscId",
                principalTable: "Wiadomosci",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
