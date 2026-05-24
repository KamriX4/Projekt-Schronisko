namespace Backend_schronisko.Models
{
    public class Zwierze
    {
        public int Id { get; set; } // Primary Key
        public string Imie { get; set; }
        public int WiekMiesiace { get; set; }
        public string ZdjecieUrl { get; set; }

        public int GatunekId { get; set; } // Foreign Key
        public Gatunek Gatunek { get; set; } // Referencja do nawigacji
    }
}
