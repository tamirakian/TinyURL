namespace TinyURLApp.Services;

public interface IUrlShorteningService
{
    Task<string> GenerateAsync(string originalUrl);
    Task<string?> GetOriginalAsync(string shortUrl);
}
