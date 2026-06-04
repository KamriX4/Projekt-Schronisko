using Backend_schronisko.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_schronisko.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HarmonogramController : ControllerBase
    {
        private readonly SchroniskoContext _context;

        public HarmonogramController(SchroniskoContext context)
        {
            _context = context;
        }

        // GET: api/harmonogram
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ZadanieHarmonogramu>>> GetZadania()
        {
            return await _context.ZadaniaHarmonogramu
                .Include(z => z.Zwierze) // Pobiera też dane zwierzaka, jeśli jest przypisany
                .OrderBy(z => z.DataCzas)
                .ToListAsync();
        }

        // POST: api/harmonogram
        [HttpPost]
        public async Task<ActionResult<ZadanieHarmonogramu>> PostZadanie(ZadanieHarmonogramu zadanie)
        {
            _context.ZadaniaHarmonogramu.Add(zadanie);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetZadania), new { id = zadanie.Id }, zadanie);
        }

        // PUT: api/harmonogram/5/zrobione
        [HttpPut("{id}/zrobione")]
        public async Task<IActionResult> OznaczJakoZrobione(int id)
        {
            var zadanie = await _context.ZadaniaHarmonogramu.FindAsync(id);
            if (zadanie == null) return NotFound();

            zadanie.CzyWykonane = !zadanie.CzyWykonane; // Przełącza status
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EdytujZadanie(int id, ZadanieHarmonogramu zadanie)
        {
            if (id != zadanie.Id) return BadRequest();

            _context.Entry(zadanie).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        // DELETE: api/harmonogram/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZadanie(int id)
        {
            var zadanie = await _context.ZadaniaHarmonogramu.FindAsync(id);
            if (zadanie == null) return NotFound();

            _context.ZadaniaHarmonogramu.Remove(zadanie);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}