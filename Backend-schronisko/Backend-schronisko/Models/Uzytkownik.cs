namespace Backend_schronisko.Models
{
    public class Uzytkownik
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Haslo { get; set; } = string.Empty; // bez szyfrowania
        public string Rola { get; set; } = "pracownik";
    }
}