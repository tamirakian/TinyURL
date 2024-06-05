using TinyURLApp.Data.Repositories;

namespace TinyURLApp.Services;

public class UrlShorteningService : IUrlShorteningService
{
    private readonly ITinyUrlDatabaseRepository _repository;
    private readonly Random _random;
    private const string BaseUrl = "https://tinyurl.com";
    private const int ShortUrlPathLength = 8;
    private static readonly char[] ShortUrlChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
    private static readonly int ShortUrlCharsLength = ShortUrlChars.Length;


    public UrlShorteningService(ITinyUrlDatabaseRepository tinyUrlDatabaseRepository)
    {
        _repository = tinyUrlDatabaseRepository;
        _random = new Random();
    }

    public async Task<string> GenerateAsync(string originalUrl)
    {
        var shortUrlMetadata = await _repository.GetShortUrlMetadataAsync(originalUrl);
        if (shortUrlMetadata != null)
        {
            return $"{BaseUrl}/{shortUrlMetadata.ShortUrl}";
        }

        var shortUrl = GenerateShortUrl();
        await _repository.SaveAsync(originalUrl, shortUrl);
        return $"{BaseUrl}/{shortUrl}"; ;
    }

    public async Task<string> GetOriginalAsync(string shortUrl)
    {
        var shortUrlPath = shortUrl.Replace(BaseUrl, "");
        var originalUrlMetadata = await _repository.GetOriginalUrlMetadataAsync(shortUrlPath);
        return originalUrlMetadata != null ? originalUrlMetadata.OriginalUrl : "";
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
