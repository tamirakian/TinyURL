using TinyURLApp.Data.Repositories;
using TinyURLApp.WebApi.Utils;

namespace TinyURLApp.Services;

public class UrlShorteningService : IUrlShorteningService
{
    private readonly ITinyUrlDatabaseRepository _repository;
    private readonly ICacheService<string, string> _cacheService;
    private readonly Random _random;
    private const int ShortUrlPathLength = 8;
    // Characters allowed in the short URL path (URL-safe)
    private static readonly char[] ShortUrlChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
    private static readonly int ShortUrlCharsLength = ShortUrlChars.Length;


    public UrlShorteningService(ITinyUrlDatabaseRepository tinyUrlDatabaseRepository, ICacheService<string, string> cacheService)
    {
        _repository = tinyUrlDatabaseRepository;
        _random = new Random();
        _cacheService = cacheService;
    }

    /*
     * Generates a short URL for the given original URL and saves it to the database.
     * If the original URL already has a corresponding short URL, returns the existing short URL.
     * 
     * Parameters:
     *   - originalUrl: The original URL to generate a short URL for.
     * 
     * Returns:
     *   A short URL corresponding to the original URL.
     */
    public async Task<string?> GenerateAsync(string originalUrl)
    {
        if (!UrlValidator.IsValidUrl(originalUrl))
        {
            return null;
        }

        var shortUrlMetadata = await _repository.GetShortUrlMetadataAsync(originalUrl);
        if (shortUrlMetadata != null)
        {
            return shortUrlMetadata.ShortUrl;
        }

        var shortUrl = GenerateShortUrl();
        await _repository.SaveAsync(originalUrl, shortUrl);
        _cacheService.Add(shortUrl, originalUrl);
        return shortUrl;
    }

    /*
     * Retrieves the original URL corresponding to the given short URL.
     * First checks the cache for the original URL. If not found, queries the database.
     * 
     * Parameters:
     *   - shortUrl: The short URL to retrieve the original URL for.
     * 
     * Returns:
     *   The original URL corresponding to the short URL, or null if not found.
     */
    public async Task<string?> GetOriginalAsync(string shortUrl)
    {
        var originalUrl = _cacheService.Get(shortUrl);
        if (originalUrl != null)
        {
            return originalUrl;
        }

        var originalUrlMetadata = await _repository.GetOriginalUrlMetadataAsync(shortUrl);
        if (originalUrlMetadata != null)
        {
            originalUrl = originalUrlMetadata.OriginalUrl;
            _cacheService.Add(shortUrl, originalUrl);
        }
        return originalUrl;
    }

    /*
     * Generates a short URL path of specified length using random characters.
     * By using a random approach, the likelihood of collisions occurring between different long URLs and their corresponding short URLs is greatly reduced.
     * 
     * Returns:
     *   A randomly generated short URL path.
     */
    private string GenerateShortUrl()
    {
        var shortUrlPath = new char[ShortUrlPathLength];
        for (int i = 0; i < ShortUrlPathLength; i++)
        {
            shortUrlPath[i] = ShortUrlChars[_random.Next(ShortUrlCharsLength)];
        }
        return new string(shortUrlPath);
    }
}
