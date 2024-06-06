namespace TinyURLApp.Models;

public class CacheItem<TKey, TValue>
{
    public TKey Key { get; }
    public TValue Value { get; }

    public CacheItem(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}
