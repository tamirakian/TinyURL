namespace TinyURLApp.Test;

public class LruCacheServiceTest
{
    [Fact]
    public void LruCacheServiceGetItemNotInCacheTest()
    {
        var cache = new LruCacheService<string, string>(2);
        var result = cache.Get("none");
        Assert.Null(result);
    }

    [Fact]
    public void LruCacheServiceAddItemTest()
    {
        var cache = new LruCacheService<string, string>(2);
        cache.Add("key", "value");
        var result = cache.Get("key");
        Assert.Equal("value", result);
    }

    [Fact]
    public void LruCacheServiceAddItemExceedsCapacityTest()
    {
        var cache = new LruCacheService<string, string>(2);
        cache.Add("key1", "value1");
        cache.Add("key2", "value2");
        cache.Add("key3", "value3");

        var result1 = cache.Get("key1");
        var result2 = cache.Get("key2");
        var result3 = cache.Get("key3");

        Assert.Null(result1);
        Assert.Equal("value2", result2);
        Assert.Equal("value3", result3);
    }
}