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

        /// <summary>Pobiera listę wszystkich zwierząt.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Zwierze>>> PobierzWszystkie()
        {
            var zwierzeta = await _context.Zwierzeta
                .AsNoTracking()
                .ToListAsync();

            return Ok(zwierzeta);
        }

        /// <summary>Dodaje nowe zwierzę do bazy danych.</summary>
        [HttpPost]
        public async Task<ActionResult<Zwierze>> Utworz([FromBody] Zwierze zwierze)
        {
            if (!await _context.Gatunki.AnyAsync(g => g.Id == zwierze.GatunekId))
            {
                return BadRequest(new { komunikat = $"Gatunek o id {zwierze.GatunekId} nie istnieje." });
            }

            zwierze.Id = 0;
            zwierze.Gatunek = null!;

            _context.Zwierzeta.Add(zwierze);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Pobierz), new { id = zwierze.Id }, zwierze);
        }

        /// <summary>Aktualizuje istniejące zwierzę.</summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<Zwierze>> Edytuj(int id, [FromBody] Zwierze zwierze)
        {
            if (id != zwierze.Id)
            {
                return BadRequest(new { komunikat = "Id w adresie URL nie zgadza się z id w treści żądania." });
            }

            if (!await _context.Gatunki.AnyAsync(g => g.Id == zwierze.GatunekId))
            {
                return BadRequest(new { komunikat = $"Gatunek o id {zwierze.GatunekId} nie istnieje." });
            }

            var istniejace = await _context.Zwierzeta.FindAsync(id);
            if (istniejace is null)
            {
                return NotFound(new { komunikat = $"Zwierzę o id {id} nie zostało znalezione." });
            }

            _context.Entry(istniejace).CurrentValues.SetValues(zwierze);
            istniejace.Gatunek = null!;

            await _context.SaveChangesAsync();

            return Ok(istniejace);
        }

        /// <summary>Usuwa zwierzę z bazy danych.</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Usun(int id)
        {
            var zwierze = await _context.Zwierzeta.FindAsync(id);
            if (zwierze is null)
            {
                return NotFound(new { komunikat = $"Zwierzę o id {id} nie zostało znalezione." });
            }

            _context.Zwierzeta.Remove(zwierze);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>Pobiera jedno zwierzę (używane m.in. przez CreatedAtAction).</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Zwierze>> Pobierz(int id)
        {
            var zwierze = await _context.Zwierzeta
                .AsNoTracking()
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zwierze is null)
            {
                return NotFound(new { komunikat = $"Zwierzę o id {id} nie zostało znalezione." });
            }

            return Ok(zwierze);
        }
    }
}
