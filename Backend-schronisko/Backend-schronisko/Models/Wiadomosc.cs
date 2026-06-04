using System;

namespace Backend_schronisko.Models
{
    public class Wiadomosc
    {
        public int Id { get; set; }
        public string Tresc { get; set; } = string.Empty;
        public DateTime DataDodania { get; set; } = DateTime.Now;
        public string Nadawca { get; set; } = string.Empty; // Login pracownika, który wysyła
        public string Odbiorca { get; set; } = string.Empty; // Wszyscy lub konkretny login pracownika
		public List<Komentarz> Komentarze { get; set; } = new List<Komentarz>();
	}
}