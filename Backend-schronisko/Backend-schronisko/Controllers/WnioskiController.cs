using Microsoft.AspNetCore.Mvc;
using Backend_schronisko.Models;
using Backend_schronisko.Services; // Importujemy nasz nowy serwis email
using System.Threading.Tasks;

namespace Backend_schronisko.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WnioskiController : ControllerBase
    {
        private readonly SchroniskoContext _context;
        private readonly EmailService _emailService; // Deklarujemy serwis

        // Wstrzykujemy kontekst bazy ORAZ nasz serwis email
        public WnioskiController(SchroniskoContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost("adopcja")]
        public async Task<IActionResult> ZlozWniosekAdopcyjny([FromBody] WniosekAdopcyjny wniosek)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 1. Zapis do bazy danych
            _context.WnioskiAdopcyjne.Add(wniosek);
            await _context.SaveChangesAsync();

            // 2. Prawdziwa wysyłka e-mail z użyciem await (żeby wychwycić błędy)
            string temat = "Nowy wniosek adopcyjny!";
            string tresc = $@"
                <h3>Otrzymano nowy wniosek adopcyjny</h3>
                <b>Od:</b> {wniosek.ImieINazwisko} <br>
                <b>Email:</b> {wniosek.Email} <br>
                <b>Telefon:</b> {wniosek.Telefon} <br>
                <b>ID Zwierzaka:</b> {wniosek.ZwierzeId} <br>
                <b>Uzasadnienie:</b> {wniosek.Uzasadnienie}
            ";

            await _emailService.WyslijPowiadomienieAsync(temat, tresc);

            // 3. Odpowiedź dla Frontendu
            return Ok(new { message = "Wniosek adopcyjny został pomyślnie zapisany!" });
        }

        [HttpPost("oddanie")]
        public async Task<IActionResult> ZlozWniosekOddania([FromBody] WniosekOddania wniosek)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 1. Zapis do bazy danych
            _context.WnioskiOddania.Add(wniosek);
            await _context.SaveChangesAsync();

            // 2. Prawdziwa wysyłka e-mail
            string temat = "Zgłoszenie oddania zwierzaka";
            string tresc = $@"
                <h3>Ktoś chce oddać zwierzaka</h3>
                <b>Zgłaszający:</b> {wniosek.ImieINazwiskoOddajacego} <br>
                <b>Imię zwierzaka:</b> {wniosek.ImieZwierzaka} ({wniosek.Gatunek}) <br>
                <b>Powód:</b> {wniosek.PowodOddania} <br>
                <b>Kontakt:</b> {wniosek.Telefon}, {wniosek.Email}
            ";

            await _emailService.WyslijPowiadomienieAsync(temat, tresc);

            // 3. Odpowiedź dla Frontendu
            return Ok(new { message = "Wniosek o oddanie został pomyślnie zapisany!" });
        }
    }
}