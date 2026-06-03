using Backend_schronisko.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_schronisko.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WiadomosciController : ControllerBase
    {
        private readonly SchroniskoContext _context;

        public WiadomosciController(SchroniskoContext context)
        {
            _context = context;
        }

        [HttpGet("{loginUzytkownika}")]
        public async Task<ActionResult<IEnumerable<Wiadomosc>>> GetWiadomosci(string loginUzytkownika)
        {
            return await _context.Wiadomosci
                .Include(w => w.Komentarze) // DODANE: żeby komentarze pobrały się razem z wiadomością
                .Where(w => w.Odbiorca == "Wszyscy" || w.Odbiorca == loginUzytkownika || w.Nadawca == loginUzytkownika)
                .OrderByDescending(w => w.DataDodania)
                .ToListAsync();
        }

        // POST: api/Wiadomosci
        [HttpPost]
        public async Task<ActionResult<Wiadomosc>> PostWiadomosc(Wiadomosc wiadomosc)
        {
            _context.Wiadomosci.Add(wiadomosc);
            await _context.SaveChangesAsync();
            return Ok(wiadomosc);
        }

        // DELETE: api/Wiadomosci/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWiadomosc(int id, [FromQuery] string loginUzytkownika)
        {
            var msg = await _context.Wiadomosci.FindAsync(id);
            if (msg == null || msg.Nadawca != loginUzytkownika) return Forbid(); // Tylko nadawca!
            _context.Wiadomosci.Remove(msg);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Wiadomosci/5/komentarz
        [HttpPost("{id}/komentarz")]
        public async Task<IActionResult> DodajKomentarz(int id, Komentarz kom)
        {
            kom.WiadomoscId = id;
            _context.Komentarze.Add(kom);
            await _context.SaveChangesAsync();
            return Ok(kom);
        }
    }
}