using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_schronisko.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gatunki",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gatunki", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zwierzeta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WiekMiesiace = table.Column<int>(type: "int", nullable: false),
                    ZdjecieUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GatunekId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zwierzeta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zwierzeta_Gatunki_GatunekId",
                        column: x => x.GatunekId,
                        principalTable: "Gatunki",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Zwierzeta_GatunekId",
                table: "Zwierzeta",
                column: "GatunekId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Zwierzeta");

            migrationBuilder.DropTable(
                name: "Gatunki");
        }
    }
}
