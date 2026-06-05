using System;
using Backend_schronisko.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_schronisko.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ZwierzeController : ControllerBase
    {
        private readonly SchroniskoContext _context;

        public ZwierzeController(SchroniskoContext context)
        {
            _context = context;
        }

        /// READ
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Zwierze>>> PobierzWszystkie()
        {
            try
            {
                var zwierzeta = await _context.Zwierzeta
                    .Include(z => z.Gatunek)
                    .AsNoTracking()
                    .ToListAsync();

                return Ok(zwierzeta); // Kod 200 sukces, odsyłanie danych
            }
            catch (Exception ex)
            {
                // Kod 500 - Błąd wewnętrzny serwera
                return StatusCode(500, new { komunikat = "Wystąpił błąd podczas pobierania listy zwierząt.", szczegoly = ex.Message });
            }
        }

        /// CREATE
        [HttpPost]
        public async Task<ActionResult<Zwierze>> Utworz([FromBody] Zwierze zwierze)
        {
            try
            {
                if (!await _context.Gatunki.AnyAsync(g => g.Id == zwierze.GatunekId))
                {
                    return BadRequest(new { komunikat = $"Gatunek o id {zwierze.GatunekId} nie istnieje." }); // Kod 400 - złe zapytanie
                }

                zwierze.Id = 0;
                zwierze.Gatunek = null!;

                _context.Zwierzeta.Add(zwierze);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Pobierz), new { id = zwierze.Id }, zwierze); // Kod 201 - utworzono
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { komunikat = "Wystąpił błąd podczas zapisywania nowego zwierzaka.", szczegoly = ex.Message });
            }
        }

        /// UPDATE
        [HttpPut("{id}")]
        public async Task<ActionResult<Zwierze>> Edytuj(int id, [FromBody] Zwierze zwierze)
        {
            try
            {
                if (id != zwierze.Id)
                {
                    return BadRequest(new { komunikat = "Id w adresie URL nie zgadza się z id w treści żądania." }); // Kod 400 - złe zapytanie
                }

                if (!await _context.Gatunki.AnyAsync(g => g.Id == zwierze.GatunekId))
                {
                    return BadRequest(new { komunikat = $"Gatunek o id {zwierze.GatunekId} nie istnieje." }); // Kod 400 - złe zapytanie
                }

                var istniejace = await _context.Zwierzeta.FindAsync(id);
                if (istniejace is null)
                {
                    return NotFound(new { komunikat = $"Zwierzę o id {id} nie zostało znalezione." }); // Kod 404 - nie znaleziono
                }

                _context.Entry(istniejace).CurrentValues.SetValues(zwierze);
                istniejace.Gatunek = null!;

                await _context.SaveChangesAsync();

                return Ok(istniejace); // Kod 200 - sukces
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { komunikat = "Wystąpił błąd podczas aktualizacji danych zwierzaka.", szczegoly = ex.Message });
            }
        }

        /// DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Usun(int id)
        {
            try
            {
                var zwierze = await _context.Zwierzeta.FindAsync(id);
                if (zwierze is null)
                {
                    return NotFound(new { komunikat = $"Zwierzę o id {id} nie zostało znalezione." }); // Kod 404 - nie znaleziono
                }

                _context.Zwierzeta.Remove(zwierze);
                await _context.SaveChangesAsync();

                return NoContent(); // Kod 204 - brak zawartosci (usunięto pomyślnie)
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { komunikat = "Wystąpił błąd podczas usuwania zwierzaka.", szczegoly = ex.Message });
            }
        }

        /// READ pobieranie szczegółowych danych o zwierzaku
        [HttpGet("{id}")]
        public async Task<ActionResult<Zwierze>> Pobierz(int id)
        {
            try
            {
                var zwierze = await _context.Zwierzeta
                    .AsNoTracking()
                    .FirstOrDefaultAsync(z => z.Id == id);

                if (zwierze is null)
                {
                    return NotFound(new { komunikat = $"Zwierzę o id {id} nie zostało znalezione." }); // Kod 404 - nie znaleziono
                }

                return Ok(zwierze); // Kod 200 - sukces
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { komunikat = "Wystąpił błąd podczas pobierania danych zwierzaka.", szczegoly = ex.Message });
            }
        }
    }
}