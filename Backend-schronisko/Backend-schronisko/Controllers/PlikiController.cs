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

        // Tworzymy folder wwwroot/images jeśli nie istnieje

        string sciezkaGlowna = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");

        // Tworzymy ścieżkę do folderu images
        string folderZapisu = Path.Combine(sciezkaGlowna, "images");
        if (!Directory.Exists(folderZapisu))
        {
            Directory.CreateDirectory(folderZapisu);
        }

        // Generujemy unikalną nazwę pliku, żeby obrazki się nie nadpisywały (np. unique-id_pies.jpg)
        string unikalnaNazwa = $"{Guid.NewGuid()}_{Path.GetFileName(plik.FileName)}";
        string pelnaSciezka = Path.Combine(folderZapisu, unikalnaNazwa);

        using (var strumien = new FileStream(pelnaSciezka, FileMode.Create))
        {
            await plik.CopyToAsync(strumien);
        }

        // Generowanie linku URL do tego zdjęcia
        string wygenerowanyUrl = $"{Request.Scheme}://{Request.Host}/images/{unikalnaNazwa}";

        // Zwracamy wygenerowany link do frontendu Vue
        return Ok(new { url = wygenerowanyUrl });
    }
}