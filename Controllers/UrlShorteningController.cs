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

    [HttpGet]
    public async Task<List<ShortenedUrlMetadata>> Get() =>
        await _urlShorteningService.GetAsync();

    // The "{id:length(24)}" part defines a route template.
    // It means the method expects a parameter named "id" which must be exactly 24 characters long.
    [HttpGet("{id:length(24)}")]
    // The "ShortenedUrlMetadata" object wrapped in an ActionResult.
    // This allows for various HTTP responses (e.g., 200 OK, 404 Not Found).
    public async Task<ActionResult<ShortenedUrlMetadata>> Get(string id)
    {
        var shortenedUrlMetadata = await _urlShorteningService.GetAsync(id);
        if (shortenedUrlMetadata is null) 
        {
            // e.g. "404 Not Found"
            return NotFound();
        }
        // The framework will automatically serialize the "shortenedUrlMetadata"
        // object to JSON and send a "200 OK" response to the client.
        return shortenedUrlMetadata;
    }

    [HttpPost]
    // "IActionResult" allows for various types of HTTP responses.
    public async Task<IActionResult> Post(ShortenedUrlMetadata newShortenedUrlMetadata)
    {
        await _urlShorteningService.CreateAsync(newShortenedUrlMetadata);
        /*
          "CreatedAtAction" - returns a "201 Created" HTTP response.
        
          "nameof(Get)" - This is a way to tell the client where they can find the newly created resource.
        
          "new { id = newShortenedUrlMetadata.Id }" - Specifies the route values, particularly the ID of the new resource.
          This will be used to construct the URL for the Get method.

          "newShortenedUrlMetadata" - The newly created resource, which will be included in the response body.
        */
        return CreatedAtAction(nameof(Get), new { id = newShortenedUrlMetadata.Id }, newShortenedUrlMetadata);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, ShortenedUrlMetadata updatedShortenedUrlMetadata)
    {
        var shortenedUrlMetadata = await _urlShorteningService.GetAsync(id);
        if (shortenedUrlMetadata is null)
        {
            return NotFound();
        }

        updatedShortenedUrlMetadata.Id = shortenedUrlMetadata.Id;
        await _urlShorteningService.UpdateAsync(id, updatedShortenedUrlMetadata);
        // returns a "204 No Content" HTTP response,
        // indicating that the update was successful and there is no content to return in the response body.
        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var shortenedUrlMetadata = await _urlShorteningService.GetAsync(id);
        if (shortenedUrlMetadata is null)
        {
            return NotFound();
        }

        await _urlShorteningService.RemoveAsync(id);
        return NoContent();
    }
}
