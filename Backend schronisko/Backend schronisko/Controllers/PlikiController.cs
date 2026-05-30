using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System;

[Route("api/[controller]")]
[ApiController]
public class PlikiController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public PlikiController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadZdjecia(IFormFile plik)
    {
        if (plik == null || plik.Length == 0)
            return BadRequest(new { komunikat = "Nie przesłano żadnego pliku." });

        // 1. Tworzymy folder wwwroot/images jeśli nie istnieje

        // Jeśli WebRootPath jest null, pobierz ścieżkę do projektu i sam dopisz "wwwroot"
        string sciezkaGlowna = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");

        // Teraz bezpiecznie tworzymy ścieżkę do folderu images
        string folderZapisu = Path.Combine(sciezkaGlowna, "images");
        if (!Directory.Exists(folderZapisu))
        {
            Directory.CreateDirectory(folderZapisu);
        }

        // 2. Generujemy unikalną nazwę pliku, żeby obrazki się nie nadpisywały (np. unique-id_pies.jpg)
        string unikalnaNazwa = $"{Guid.NewGuid()}_{Path.GetFileName(plik.FileName)}";
        string pelnaSciezka = Path.Combine(folderZapisu, unikalnaNazwa);

        // 3. Zapisujemy plik fizycznie na dysku serwera
        using (var strumien = new FileStream(pelnaSciezka, FileMode.Create))
        {
            await plik.CopyToAsync(strumien);
        }

        // 4. Generujemy gotowy link URL do tego zdjęcia
        // Request.Scheme to np. "http", Request.Host to np. "localhost:5001"
        string wygenerowanyUrl = $"{Request.Scheme}://{Request.Host}/images/{unikalnaNazwa}";

        // 5. Zwracamy wygenerowany link do frontendu Vue
        return Ok(new { url = wygenerowanyUrl });
    }
}