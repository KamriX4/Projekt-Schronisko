using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_schronisko.Migrations
{
    /// <inheritdoc />
    public partial class AktualizacjaKlasyZwierze : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WiekMiesiace",
                table: "Zwierzeta");

            migrationBuilder.AlterColumn<string>(
                name: "ZdjecieUrl",
                table: "Zwierzeta",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Imie",
                table: "Zwierzeta",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Akceptuje",
                table: "Zwierzeta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Choroby",
                table: "Zwierzeta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CzyKastrowanySterylizowany",
                table: "Zwierzeta",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CzySzczepiony",
                table: "Zwierzeta",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CzyZachipowany",
                table: "Zwierzeta",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataPrzyjecia",
                table: "Zwierzeta",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "NumerBoksu",
                table: "Zwierzeta",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumerChip",
                table: "Zwierzeta",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumerEwidencyjny",
                table: "Zwierzeta",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Plec",
                table: "Zwierzeta",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PrzyblizonaDataUrodzenia",
                table: "Zwierzeta",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Rasa",
                table: "Zwierzeta",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Zwierzeta",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Umaszczenie",
                table: "Zwierzeta",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Wielkosc",
                table: "Zwierzeta",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zachowanie",
                table: "Zwierzeta",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Akceptuje",
                table: "Zwierzeta");

            migrationBuilder.DropColumn(
                name: "Choroby",
                table: "Zwierzeta");

            migrationBuilder.DropColumn(
                name: "CzyKastrowanySterylizowany",
                table: "Zwierzeta");

            migrationBuilder.DropColumn(
                name: "CzySzczepiony",
                table: "Zwierzeta");

            migrationBuilder.DropColumn(
                name: "CzyZachipowany",
                table: "Zwierzeta");

            migrationBuilder.DropColumn(
                name: "DataPrzyjecia",
                table: "Zwierzeta");

            migrationBuilder.DropColumn(
                name: "NumerBoksu",
                table: "Zwierzeta");

            migrationBuilder.DropColumn(
                name: "NumerChip",
                table: "Zwierzeta");

            migrationBuilder.DropColumn(
                name: "NumerEwidencyjny",
                table: "Zwierzeta");

            migrationBuilder.DropColumn(
                name: "Plec",
                table: "Zwierzeta");

            migrationBuilder.DropColumn(
                name: "PrzyblizonaDataUrodzenia",
                table: "Zwierzeta");

            migrationBuilder.DropColumn(
                name: "Rasa",
                table: "Zwierzeta");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Zwierzeta");

            migrationBuilder.DropColumn(
                name: "Umaszczenie",
                table: "Zwierzeta");

            migrationBuilder.DropColumn(
                name: "Wielkosc",
                table: "Zwierzeta");

            migrationBuilder.DropColumn(
                name: "Zachowanie",
                table: "Zwierzeta");

            migrationBuilder.AlterColumn<string>(
                name: "ZdjecieUrl",
                table: "Zwierzeta",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Imie",
                table: "Zwierzeta",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "WiekMiesiace",
                table: "Zwierzeta",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
