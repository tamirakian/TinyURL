using Microsoft.AspNetCore.Mvc;
using TinyURLApp.Services;

namespace TinyURLApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlShorteningController : ControllerBase
{
    private readonly IUrlShorteningService _urlShorteningService;

    public UrlShorteningController(IUrlShorteningService urlShorteningService) =>
        _urlShorteningService = urlShorteningService;

    [HttpPost]
    public async Task<ActionResult> Shorten(string originalUrl)
    {
        var shortenedUrl = await _urlShorteningService.GenerateAsync(originalUrl);
        return Ok(shortenedUrl);
    }

    [HttpGet("{shortUrl}")]
    public async Task<IActionResult> RedirectToOriginal([FromRoute] string shortUrl)
    {
        var originalUrl = await _urlShorteningService.GetOriginalAsync(shortUrl);
        if (originalUrl is null) 
        {
            return NotFound();
        }
        return Redirect(originalUrl);
    }
}
