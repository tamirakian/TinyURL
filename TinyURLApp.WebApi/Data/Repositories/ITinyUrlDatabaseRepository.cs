using TinyURLApp.Models;

namespace TinyURLApp.Data.Repositories;

public interface ITinyUrlDatabaseRepository
{
    Task<UrlMetadata> GetShortUrlMetadataAsync(string originalUrl);
    Task<UrlMetadata> GetOriginalUrlMetadataAsync(string shortUrl);
    Task SaveAsync(string originalUrl, string shortUrl);
}
