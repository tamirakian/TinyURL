namespace TinyURLApp.Services;

public interface ICacheService<TKey, TValue>
{
    TValue? Get(TKey key);
    void Add(TKey key, TValue value);
    void Clear();
}
