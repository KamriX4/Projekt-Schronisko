using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_schronisko.Models
{
    public class Zwierze
    {
        // --- PODSTAWOWE ID I RELACJE ---
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Imie { get; set; }

        [Required]
        public int GatunekId { get; set; }
        public Gatunek? Gatunek { get; set; } // Właściwość nawigacyjna


        // --- PROPOZYCJE LOGISTYCZNE I BIZNESOWE ---
        [Required]
        [MaxLength(30)]
        public string NumerEwidencyjny { get; set; } // Np. "SCHR/2026/05/001"

        [Required]
        public string Status { get; set; } // Np. Kwarantanna, Do Adopcji, Adoptowany

        [MaxLength(20)]
        public string? NumerBoksu { get; set; } // Lokalizacja w schronisku

        [Required]
        public DateTime DataPrzyjecia { get; set; }


        // --- CECHY ELEMENTARNE I IDENTYFIKACJA ---
        [Required]
        public string Plec { get; set; } // Np. Samiec / Samica
        
        [Required]
        public DateTime PrzyblizonaDataUrodzenia { get; set; }

        // 2. Tego NIE zapisujemy w bazie. C# wyliczy to sam, gdy Vue o to poprosi.
        [NotMapped]
        public int WiekMiesiace
        {
            get
            {
                // Prosta matematyka: różnica między dzisiaj a datą urodzenia
                var roznica = DateTime.Now - PrzyblizonaDataUrodzenia;
                return (int)(roznica.Days / 30.436875); // Dzielimy przez średnią długość miesiąca
            }
        }

        [MaxLength(100)]
        public string? Rasa { get; set; } // Dla nierasowych można wpisać "Mieszaniec"

        public bool CzyZachipowany { get; set; }

        [MaxLength(50)]
        public string? NumerChip { get; set; } // Widoczny tylko jeśli CzyZachipowany == true


        // --- CECHY SPECYFICZNE DLA GATUNKÓW ---
        [MaxLength(50)]
        public string? Umaszczenie { get; set; } // Głównie dla kotów (np. tricolor, pręgowany)

        [MaxLength(30)]
        public string? Wielkosc { get; set; } // Głównie dla psów (np. Mały, Średni, Olbrzymi)


        // --- ZDROWIE I PROFILE MEDYCZNE ---
        public bool CzySzczepiony { get; set; }

        public bool CzyKastrowanySterylizowany { get; set; }

        public string? Choroby { get; set; } // Opis stałych dolegliwości lub leków


        // --- BEHAWIOR I ADOPCJA ---
        public string? Zachowanie { get; set; } // Np. "Lękliwy, reaguje agresją na smycz"

        public string? Akceptuje { get; set; } // Np. "Akceptuje suczki, nie akceptuje kotów"

        public string? ZdjecieUrl { get; set; }
    }
}
