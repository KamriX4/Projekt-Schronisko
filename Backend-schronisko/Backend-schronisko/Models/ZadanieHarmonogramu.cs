namespace Backend_schronisko.Models
{
    public class ZadanieHarmonogramu
    {
        public int Id { get; set; }
        public string Tytul { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        
        // Tutaj będziemy trzymać jedną z 4 wartości: "Zywienie", "Szczepienia", "Leki", "Sprzatanie"
        public string Kategoria { get; set; } = string.Empty; 
        
        public DateTime DataCzas { get; set; }
        public bool CzyWykonane { get; set; } = false;

        // Opcjonalne powiązanie ze zwierzakiem (przydatne przy lekach i szczepieniach)
        public int? ZwierzeId { get; set; }
        public Zwierze? Zwierze { get; set; }
    }
}