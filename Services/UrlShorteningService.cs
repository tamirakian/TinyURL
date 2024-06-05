using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;
using TinyURLApp.Models;

namespace TinyURLApp.Services;

public class UrlShorteningService
{
    private readonly IMongoCollection<ShortenedUrlMetadata> _shortenedUrlsMetadataCollection;
    private readonly Random _random;
    private const string BaseUrl = "https://tinyurl.com";
    private const int ShortUrlLength = 8;
    private static readonly char[] ShortUrlChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
    private static readonly int ShortUrlCharsLength = ShortUrlChars.Length;


    public UrlShorteningService(IOptions<TinyUrlDatabaseSettings> tinyUrlDatabaseSettings)
    {
        var mongoClient = new MongoClient(tinyUrlDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(tinyUrlDatabaseSettings.Value.DatabaseName);
        _shortenedUrlsMetadataCollection = mongoDatabase.GetCollection<ShortenedUrlMetadata>(
            tinyUrlDatabaseSettings.Value.ShortenedUrlsMetadataCollectionName);
        _random = new Random();
    }

/*    // The "_ => true" is a lambda expression used as a filter, selecting all documents in the collection.
    public async Task<List<ShortenedUrlMetadata>> GetAsync() =>
        await _shortenedUrlsMetadataCollection.Find(_ => true).ToListAsync();

    public async Task<ShortenedUrlMetadata?> GetAsync(string id) =>
        await _shortenedUrlsMetadataCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(ShortenedUrlMetadata newShortenedUrlMetadata) =>
        await _shortenedUrlsMetadataCollection.InsertOneAsync(newShortenedUrlMetadata);

    public async Task UpdateAsync(string id, ShortenedUrlMetadata updatedShortenedUrlMetadata) =>
        await _shortenedUrlsMetadataCollection.ReplaceOneAsync(x => x.Id == id, updatedShortenedUrlMetadata);

    public async Task RemoveAsync(string id) =>
        await _shortenedUrlsMetadataCollection.DeleteOneAsync(x => x.Id == id);*/

    public async Task<string> GetShortAsync(string originalUrl)
    {
        var shortenedUrlMetadata = await _shortenedUrlsMetadataCollection.Find(x => x.OriginalUrl == originalUrl).FirstOrDefaultAsync();
        return shortenedUrlMetadata != null ? shortenedUrlMetadata.ShortenedUrl : "";
    }

    public async Task<string> GetOriginalAsync(string shortUrl)
    {
        var originalUrlMetadata = await _shortenedUrlsMetadataCollection.Find(x => x.ShortenedUrl == shortUrl).FirstOrDefaultAsync();
        return originalUrlMetadata != null ? originalUrlMetadata.OriginalUrl : "";
    }

    public async Task SaveAsync(string originalUrl, string shortenedUrl)
    {
        await _shortenedUrlsMetadataCollection.InsertOneAsync(new ShortenedUrlMetadata {  OriginalUrl = originalUrl, ShortenedUrl = shortenedUrl });
    }

    public async Task<string> GenerateAsync(string originalUrl)
    {
        var existingShortUrl = await GetShortAsync(originalUrl);
        if (existingShortUrl != null && existingShortUrl != "")
        {
            return existingShortUrl;
        }

        /*// Generating short URL using MD5 hash function (32 characters long)
        byte[] bytes = MD5.HashData(Encoding.UTF8.GetBytes(originalUrl));
        StringBuilder hash = new();
        foreach (byte b in bytes)
        {
            hash.Append(b.ToString("x2"));
        }*/
        var shortUrlPath = new char[ShortUrlLength];
        for (int i = 0; i < ShortUrlLength; i++)
        {
            shortUrlPath[i] = ShortUrlChars[_random.Next(ShortUrlCharsLength)];
        }

        var shortUrl = string.Format("{0}/{1}", BaseUrl, new string(shortUrlPath));
        await SaveAsync(originalUrl, shortUrl);
        return shortUrl;
    }
}
