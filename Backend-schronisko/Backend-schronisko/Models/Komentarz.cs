public class Komentarz
{
    public int Id { get; set; }
    public string Tresc { get; set; }
    public string Autor { get; set; }
    public DateTime DataDodania { get; set; } = DateTime.Now;
    public int WiadomoscId { get; set; }
}