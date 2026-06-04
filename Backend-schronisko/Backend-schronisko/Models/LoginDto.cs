namespace Backend_schronisko.Models
{
    public class LoginDto // wysyłamy dane za pomoca wzorca dto
    {
        public string Login { get; set; } = string.Empty;
        public string Haslo { get; set; } = string.Empty;
    }
}