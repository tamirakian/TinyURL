using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TinyURLApp.Models;

namespace TinyURLApp.Services;

public class UrlShorteningService
{
    private readonly IMongoCollection<ShortenedUrlMetadata> _shortenedUrlsMetadataCollection;

    public UrlShorteningService(IOptions<TinyUrlDatabaseSettings> tinyUrlDatabaseSettings)
    {
        var mongoClient = new MongoClient(tinyUrlDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(tinyUrlDatabaseSettings.Value.DatabaseName);
        _shortenedUrlsMetadataCollection = mongoDatabase.GetCollection<ShortenedUrlMetadata>(
            tinyUrlDatabaseSettings.Value.ShortenedUrlsMetadataCollectionName);
    }

    // The "_ => true" is a lambda expression used as a filter, selecting all documents in the collection.
    public async Task<List<ShortenedUrlMetadata>> GetAsync() =>
        await _shortenedUrlsMetadataCollection.Find(_ => true).ToListAsync();

    public async Task<ShortenedUrlMetadata?> GetAsync(string id) =>
        await _shortenedUrlsMetadataCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(ShortenedUrlMetadata newShortenedUrlMetadata) =>
        await _shortenedUrlsMetadataCollection.InsertOneAsync(newShortenedUrlMetadata);

    public async Task UpdateAsync(string id, ShortenedUrlMetadata updatedShortenedUrlMetadata) =>
        await _shortenedUrlsMetadataCollection.ReplaceOneAsync(x => x.Id == id, updatedShortenedUrlMetadata);

    public async Task RemoveAsync(string id) =>
        await _shortenedUrlsMetadataCollection.DeleteOneAsync(x => x.Id == id);
}
