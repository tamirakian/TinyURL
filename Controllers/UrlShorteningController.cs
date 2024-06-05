using Microsoft.AspNetCore.Mvc;
using TinyURLApp.Models;
using TinyURLApp.Services;

namespace TinyURLApp.Controllers;

// [ApiController] - a shorthand way of enabling various features in ASP.NET Core,
// such as automatic model validation, binding source parameter inference, and error handling.
[ApiController]
// it indicates that the controller will handle requests under the "/api/urlshortening" route,
// where "urlshortening" is the normalized name of the controller class ("UrlShorteningController").
// The [controller] token in the route template will be replaced with the name of the controller class without the "Controller" suffix.
[Route("api/[controller]")]
// ControllerBase is a base class provided by ASP.NET Core for API controllers.
// It provides common functionality such as action methods, request context, and access to services through dependency injection.
public class UrlShorteningController : ControllerBase
{
    private readonly UrlShorteningService _urlShorteningService;

    public UrlShorteningController(UrlShorteningService urlShorteningService) =>
        _urlShorteningService = urlShorteningService;

    [HttpPost]
    public async Task<ActionResult> Shorten(string originalUrl)
    {
        var shortenedUrl = await _urlShorteningService.GenerateAsync(originalUrl);
        return Ok(shortenedUrl);
    }

    [HttpGet]
    // The "ShortenedUrlMetadata" object wrapped in an ActionResult.
    // This allows for various HTTP responses (e.g., 200 OK, 404 Not Found).
    public async Task<IActionResult> RedirectShort(string shortUrl)
    {
        var originalUrl = await _urlShorteningService.GetOriginalAsync(shortUrl);
        if (originalUrl is null) 
        {
            return NotFound();
        }
        return Redirect(originalUrl);
    }
}
