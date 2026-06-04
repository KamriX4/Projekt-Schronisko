using Microsoft.EntityFrameworkCore;

namespace Backend_schronisko.Models
{
    public class SchroniskoContext : DbContext
    {
        public SchroniskoContext(DbContextOptions<SchroniskoContext> options) : base(options)
        {

        }
        // Te właściwości reprezentują tabele w bazie MSSQL
        public DbSet<Zwierze> Zwierzeta { get; set; }
        public DbSet<Gatunek> Gatunki { get; set; }
        public DbSet<WniosekAdopcyjny> WnioskiAdopcyjne { get; set; }
        public DbSet<WniosekOddania> WnioskiOddania { get; set; }

        // Główne miejsce do wpisywania danych na sztywno
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Wstawianie danych dla tabeli Gatunek
            modelBuilder.Entity<Gatunek>().HasData(
                new Gatunek { Id = 1, Nazwa = "Pies" },
                new Gatunek { Id = 2, Nazwa = "Kot" }
            );

            // 2. Wstawianie danych dla tabeli Zwierze (3 psy, 3 koty)
            modelBuilder.Entity<Zwierze>().HasData(
                // --- PSY (GatunekId = 1) ---
                new Zwierze
                {
                    Id = 1,
                    Imie = "Burek",
                    GatunekId = 1,
                    NumerEwidencyjny = "P-2026-001",
                    Status = "Do adopcji",
                    NumerBoksu = "B12",
                    DataPrzyjecia = new DateTime(2026, 5, 10),
                    Plec = "Samiec",
                    PrzyblizonaDataUrodzenia = new DateTime(2022, 3, 15),
                    Rasa = "Mieszaniec",
                    CzyZachipowany = true,
                    NumerChip = "982000000123456",
                    Umaszczenie = "Czarno-podpalane",
                    Wielkosc = "Średnia",
                    CzySzczepiony = true,
                    CzyKastrowanySterylizowany = true,
                    Choroby = "Brak",
                    Zachowanie = "Przyjazny, energiczny, lubi spacery",
                    Akceptuje = "Inne psy, starsze dzieci",
                    ZdjecieUrl = "https://images.unsplash.com/photo-1543466835-00a7907e9de1"
                },
                new Zwierze
                {
                    Id = 2,
                    Imie = "Luna",
                    GatunekId = 1,
                    NumerEwidencyjny = "P-2026-002",
                    Status = "W trakcie leczenia",
                    NumerBoksu = "Szpitalik",
                    DataPrzyjecia = new DateTime(2026, 5, 28),
                    Plec = "Samica",
                    PrzyblizonaDataUrodzenia = new DateTime(2025, 1, 10),
                    Rasa = "W typie owczarka",
                    CzyZachipowany = false,
                    NumerChip = null,
                    Umaszczenie = "Wilczaste",
                    Wielkosc = "Duża",
                    CzySzczepiony = false,
                    CzyKastrowanySterylizowany = false,
                    Choroby = "Niedowaga, zapalenie ucha",
                    Zachowanie = "Lękliwa, wycofana, wymaga cierpliwości",
                    Akceptuje = "Tylko dorośli",
                    ZdjecieUrl = "https://images.unsplash.com/photo-1583511655857-d19b40a7a54e"
                },
                new Zwierze
                {
                    Id = 3,
                    Imie = "Reksio",
                    GatunekId = 1,
                    NumerEwidencyjny = "P-2026-003",
                    Status = "Zarezerwowany",
                    NumerBoksu = "A4",
                    DataPrzyjecia = new DateTime(2026, 4, 1),
                    Plec = "Samiec",
                    PrzyblizonaDataUrodzenia = new DateTime(2020, 8, 20),
                    Rasa = "Jack Russell Terrier mix",
                    CzyZachipowany = true,
                    NumerChip = "982000000654321",
                    Umaszczenie = "Biało-brązowe",
                    Wielkosc = "Mała",
                    CzySzczepiony = true,
                    CzyKastrowanySterylizowany = true,
                    Choroby = "Alergia pokarmowa",
                    Zachowanie = "Bardzo aktywny, głośny, uwielbia zabawki",
                    Akceptuje = "Koty, psy, dzieci",
                    ZdjecieUrl = "https://images.unsplash.com/photo-1537151608804-ea2f1fae5e6e"
                },

                // --- KOTY (GatunekId = 2) ---
                new Zwierze
                {
                    Id = 4,
                    Imie = "Filemon",
                    GatunekId = 2,
                    NumerEwidencyjny = "K-2026-001",
                    Status = "Do adopcji",
                    NumerBoksu = "Kociarnia 1",
                    DataPrzyjecia = new DateTime(2026, 5, 15),
                    Plec = "Samiec",
                    PrzyblizonaDataUrodzenia = new DateTime(2025, 6, 1),
                    Rasa = "Europejska",
                    CzyZachipowany = true,
                    NumerChip = "982000000111222",
                    Umaszczenie = "Biało-czarne",
                    Wielkosc = "Mała",
                    CzySzczepiony = true,
                    CzyKastrowanySterylizowany = true,
                    Choroby = "Brak",
                    Zachowanie = "Miziasty, ciągle mruczy, domaga się uwagi",
                    Akceptuje = "Inne koty",
                    ZdjecieUrl = "https://images.unsplash.com/photo-1514888286974-6c03e2ca1dba"
                },
                new Zwierze
                {
                    Id = 5,
                    Imie = "Kropka",
                    GatunekId = 2,
                    NumerEwidencyjny = "K-2026-002",
                    Status = "Kwarantanna",
                    NumerBoksu = "Klatka 3",
                    DataPrzyjecia = new DateTime(2026, 5, 30),
                    Plec = "Samica",
                    PrzyblizonaDataUrodzenia = new DateTime(2024, 4, 10),
                    Rasa = "Europejska",
                    CzyZachipowany = true,
                    NumerChip = "982000000333444",
                    Umaszczenie = "Szylkretowe",
                    Wielkosc = "Średnia",
                    CzySzczepiony = false,
                    CzyKastrowanySterylizowany = true,
                    Choroby = "Katar koci (leczony)",
                    Zachowanie = "Spokojna, głównie śpi, ceni przestrzeń",
                    Akceptuje = "Brak danych",
                    ZdjecieUrl = "https://images.unsplash.com/photo-1513360371669-4adf3dd7dff8"
                },
                new Zwierze
                {
                    Id = 6,
                    Imie = "Mruczek",
                    GatunekId = 2,
                    NumerEwidencyjny = "K-2026-003",
                    Status = "Do adopcji",
                    NumerBoksu = "Kociarnia 2",
                    DataPrzyjecia = new DateTime(2026, 2, 14),
                    Plec = "Samiec",
                    PrzyblizonaDataUrodzenia = new DateTime(2018, 5, 5),
                    Rasa = "Brytyjski (mix)",
                    CzyZachipowany = true,
                    NumerChip = "982000000555666",
                    Umaszczenie = "Niebieskie",
                    Wielkosc = "Duża",
                    CzySzczepiony = true,
                    CzyKastrowanySterylizowany = true,
                    Choroby = "Zdiagnozowana niewydolność nerek - wymaga karmy Renal",
                    Zachowanie = "Wielki pieszczoch, ale tylko na własnych zasadach",
                    Akceptuje = "Wymaga bycia jedynakiem",
                    ZdjecieUrl = "https://images.unsplash.com/photo-1495360010541-f48722b34f7d"
                }
            );
        }
    }
}
