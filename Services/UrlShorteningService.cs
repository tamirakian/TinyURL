using TinyURLApp.Data.Repositories;

namespace TinyURLApp.Services;

public class UrlShorteningService : IUrlShorteningService
{
    private readonly ITinyUrlDatabaseRepository _repository;
    private readonly ICacheService<string, string> _cacheService;
    private readonly Random _random;
    private const int ShortUrlPathLength = 8;
    private static readonly char[] ShortUrlChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
    private static readonly int ShortUrlCharsLength = ShortUrlChars.Length;


    public UrlShorteningService(ITinyUrlDatabaseRepository tinyUrlDatabaseRepository, ICacheService<string, string> cacheService)
    {
        _repository = tinyUrlDatabaseRepository;
        _random = new Random();
        _cacheService = cacheService;
    }

    public async Task<string> GenerateAsync(string originalUrl)
    {
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
