using Moq;
using TinyURLApp.Data.Repositories;
using TinyURLApp.Models;

namespace TinyURLApp.Test;

public class UrlShorteningServiceTest
{
    private readonly Mock<ITinyUrlDatabaseRepository> _repositoryMock;
    private readonly Mock<ICacheService<string, string>> _cacheMock;
    private readonly IUrlShorteningService _urlShorteningService;

    public UrlShorteningServiceTest()
    {
        _repositoryMock = new Mock<ITinyUrlDatabaseRepository>();
        _cacheMock = new Mock<ICacheService<string, string>>();
        _urlShorteningService = new UrlShorteningService(
            _repositoryMock.Object,
            _cacheMock.Object
        );
    }

    [Fact]
    public async Task UrlShorteningServiceGenerateAsyncExistingUrlTest()
    {
        var originalUrl = "https://www.google.com";
        var existingShortUrl = "abc123";
        var existingMetadata = new UrlMetadata
        {
            OriginalUrl = originalUrl,
            ShortUrl = existingShortUrl
        };

        _repositoryMock.Setup(repo => repo.GetShortUrlMetadataAsync(originalUrl))
            .ReturnsAsync(existingMetadata);

        var result = await _urlShorteningService.GenerateAsync(originalUrl);
        Assert.Equal(existingShortUrl, result);
    }

    [Fact]
    public async Task UrlShorteningServiceGetOriginalAsyncExistingUrlInCacheTest()
    {
        var shortUrl = "abc123";
        var originalUrl = "https://www.google.com";
        _cacheMock.Setup(cache => cache.Get(shortUrl)).Returns(originalUrl);

        var result = await _urlShorteningService.GetOriginalAsync(shortUrl);
        Assert.Equal(originalUrl, result);
    }

    [Fact]
    public async Task UrlShorteningServiceGetOriginalAsyncExistingInDbNotInCacheTest()
    {
        var shortUrl = "abc123";
        var originalUrl = "https://www.google.com";
        var urlMetadata = new UrlMetadata
        {
            OriginalUrl = originalUrl,
            ShortUrl = shortUrl
        };

        _cacheMock.Setup(cache => cache.Get(shortUrl)).Returns((string)null);

        _repositoryMock.Setup(repo => repo.GetOriginalUrlMetadataAsync(shortUrl))
            .ReturnsAsync(urlMetadata);

        var result = await _urlShorteningService.GetOriginalAsync(shortUrl);
        Assert.Equal(originalUrl, result);
    }

    [Fact]
    public async Task UrlShorteningServiceGetOriginalAsyncNotInDbOrCacheTest()
    {
        var shortUrl = "abc123";

        _cacheMock.Setup(cache => cache.Get(shortUrl)).Returns((string)null);

        _repositoryMock.Setup(repo => repo.GetShortUrlMetadataAsync(shortUrl))
            .ReturnsAsync((UrlMetadata)null);

        var result = await _urlShorteningService.GetOriginalAsync(shortUrl);
        Assert.Null(result);
    }
}
