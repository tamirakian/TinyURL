using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TinyURLApp.Models;

namespace TinyURLApp.Data.Repositories;

public class TinyUrlDatabaseRepository : ITinyUrlDatabaseRepository
{
    private readonly IMongoCollection<UrlMetadata> _collection;

    public TinyUrlDatabaseRepository(IOptions<TinyUrlDatabaseSettings> tinyUrlDatabaseSettings)
    {
        var mongoClient = new MongoClient(tinyUrlDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(tinyUrlDatabaseSettings.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<UrlMetadata>(
            tinyUrlDatabaseSettings.Value.UrlsMetadataCollectionName);
    }

    public async Task<UrlMetadata> GetShortUrlMetadataAsync(string originalUrl)
    {
        return await _collection.Find(x => x.OriginalUrl == originalUrl).FirstOrDefaultAsync();
    }

    public async Task<UrlMetadata> GetOriginalUrlMetadataAsync(string shortUrl)
    {
        return await _collection.Find(x => x.ShortUrl == shortUrl).FirstOrDefaultAsync();
    }

    public async Task SaveAsync(string originalUrl, string shortUrl)
    {
        await _collection.InsertOneAsync(new UrlMetadata { OriginalUrl = originalUrl, ShortUrl = shortUrl });
    }
}
