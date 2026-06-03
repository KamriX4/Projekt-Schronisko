using Microsoft.AspNetCore.Mvc;
using Backend_schronisko.Models;
using System;
using System.Linq;

namespace Backend_schronisko.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SchroniskoContext _context;

        public AuthController(SchroniskoContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto daneLogowania)
        {
            // blok try-catch
            try
            {
                if (string.IsNullOrWhiteSpace(daneLogowania.Login) || string.IsNullOrWhiteSpace(daneLogowania.Haslo))
                {
                    // Zwracanie odpowiedzi z kodem i komunikatem
                    return BadRequest(new { kod = 400, komunikat = "Login i hasło nie mogą być puste." });
                }

                // Szukanie użytkownika w bazie danych MsSQL
                var uzytkownik = _context.Uzytkownicy
                    .FirstOrDefault(u => u.Login == daneLogowania.Login && u.Haslo == daneLogowania.Haslo);

                if (uzytkownik == null)
                {
                    return Unauthorized(new { kod = 401, komunikat = "Błędny login lub hasło." });
                }

                return Ok(new { kod = 200, komunikat = "Zalogowano pomyślnie", rola = uzytkownik.Rola, login = uzytkownik.Login });
            }
            catch (Exception ex)
            {
                // Obsługa błędu serwera ze zwróceniem kodu 500 i komunikatu
                return StatusCode(500, new { kod = 500, komunikat = "Wystąpił błąd po stronie serwera: " + ex.Message });
            }
        }
    }
}