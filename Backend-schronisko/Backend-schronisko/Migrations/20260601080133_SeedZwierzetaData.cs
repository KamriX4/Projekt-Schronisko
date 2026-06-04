using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend_schronisko.Migrations
{
    /// <inheritdoc />
    public partial class SeedZwierzetaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Zwierzeta",
                columns: new[] { "Id", "Akceptuje", "Choroby", "CzyKastrowanySterylizowany", "CzySzczepiony", "CzyZachipowany", "DataPrzyjecia", "GatunekId", "Imie", "NumerBoksu", "NumerChip", "NumerEwidencyjny", "Plec", "PrzyblizonaDataUrodzenia", "Rasa", "Status", "Umaszczenie", "Wielkosc", "Zachowanie", "ZdjecieUrl" },
                values: new object[,]
                {
                    { 1, "Inne psy, starsze dzieci", "Brak", true, true, true, new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Burek", "B12", "982000000123456", "P-2026-001", "Samiec", new DateTime(2022, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mieszaniec", "Do adopcji", "Czarno-podpalane", "Średnia", "Przyjazny, energiczny, lubi spacery", "https://images.unsplash.com/photo-1543466835-00a7907e9de1" },
                    { 2, "Tylko dorośli", "Niedowaga, zapalenie ucha", false, false, false, new DateTime(2026, 5, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Luna", "Szpitalik", null, "P-2026-002", "Samica", new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "W typie owczarka", "W trakcie leczenia", "Wilczaste", "Duża", "Lękliwa, wycofana, wymaga cierpliwości", "https://images.unsplash.com/photo-1583511655857-d19b40a7a54e" },
                    { 3, "Koty, psy, dzieci", "Alergia pokarmowa", true, true, true, new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Reksio", "A4", "982000000654321", "P-2026-003", "Samiec", new DateTime(2020, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jack Russell Terrier mix", "Zarezerwowany", "Biało-brązowe", "Mała", "Bardzo aktywny, głośny, uwielbia zabawki", "https://images.unsplash.com/photo-1537151608804-ea2f1fae5e6e" },
                    { 4, "Inne koty", "Brak", true, true, true, new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Filemon", "Kociarnia 1", "982000000111222", "K-2026-001", "Samiec", new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Europejska", "Do adopcji", "Biało-czarne", "Mała", "Miziasty, ciągle mruczy, domaga się uwagi", "https://images.unsplash.com/photo-1514888286974-6c03e2ca1dba" },
                    { 5, "Brak danych", "Katar koci (leczony)", true, false, true, new DateTime(2026, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Kropka", "Klatka 3", "982000000333444", "K-2026-002", "Samica", new DateTime(2024, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Europejska", "Kwarantanna", "Szylkretowe", "Średnia", "Spokojna, głównie śpi, ceni przestrzeń", "https://images.unsplash.com/photo-1513360371669-4adf3dd7dff8" },
                    { 6, "Wymaga bycia jedynakiem", "Zdiagnozowana niewydolność nerek - wymaga karmy Renal", true, true, true, new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Mruczek", "Kociarnia 2", "982000000555666", "K-2026-003", "Samiec", new DateTime(2018, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Brytyjski (mix)", "Do adopcji", "Niebieskie", "Duża", "Wielki pieszczoch, ale tylko na własnych zasadach", "https://images.unsplash.com/photo-1495360010541-f48722b34f7d" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Zwierzeta",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Zwierzeta",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Zwierzeta",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Zwierzeta",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Zwierzeta",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Zwierzeta",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
