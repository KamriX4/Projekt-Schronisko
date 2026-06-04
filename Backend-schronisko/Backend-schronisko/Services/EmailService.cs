using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Backend_schronisko.Services
{
    public class EmailService
    {
        // Ta metoda wysyła maila do schroniska z informacją o nowym formularzu
        public async Task WyslijPowiadomienieAsync(string temat, string tresc)
        {
            // 1. Konfiguracja konta, Z KTÓREGO wysyłamy e-maile (Twój "nadawca")
            string hostSmtp = "smtp.gmail.com"; // Przykład dla Gmaila
            int port = 587;
            string emailNadawcy = "radekorlikowski@gmail.com"; 
            string hasloNadawcy = "cvarbdtniadptkxg";              

            // 2. Adres schroniska, NA KTÓRY mają przychodzić powiadomienia
            string emailSchroniska = "radekorlikowski@gmail.com"; 

            // 3. Budowanie klienta poczty
            var smtpClient = new SmtpClient(hostSmtp)
            {
                Port = port,
                Credentials = new NetworkCredential(emailNadawcy, hasloNadawcy),
                EnableSsl = true,
            };

            // 4. Tworzenie samej wiadomości
            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailNadawcy, "Powiadomienia Schronisko"),
                Subject = temat,
                Body = tresc,
                IsBodyHtml = true, // Dzięki temu możemy używać pogrubień <br> i <b> w treści
            };
            mailMessage.To.Add(emailSchroniska);

            // 5. Wysłanie
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}