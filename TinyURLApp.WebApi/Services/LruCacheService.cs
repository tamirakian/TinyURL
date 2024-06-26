﻿using System.Collections.Concurrent;
using TinyURLApp.Models;

namespace TinyURLApp.Services;

public class LruCacheService<TKey, TValue> : ICacheService<TKey, TValue> where TKey : notnull
{
    private readonly int _capacity;
    private readonly ConcurrentDictionary<TKey, LinkedListNode<CacheItem<TKey, TValue>>> _cache;
    private readonly LinkedList<CacheItem<TKey, TValue>> _cacheItemsOrder;
    private readonly object _lock = new();

    public LruCacheService(int capacity)
    {
        _capacity = capacity > 0 ? capacity : 
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than 0");
        _cache = new ConcurrentDictionary<TKey, LinkedListNode<CacheItem<TKey, TValue>>>();
        _cacheItemsOrder = new LinkedList<CacheItem<TKey, TValue>>();
    }

    public TValue? Get(TKey key)
    {
        if (_cache.TryGetValue(key, out var node))
        {
            // Lock the cache to ensure thread safety while modifying the linked list.
            lock (_lock)
            {
                // Move the accessed node to the front of the linked list to mark it as recently used.
                _cacheItemsOrder.Remove(node);
                _cacheItemsOrder.AddFirst(node);
            }
            return node.Value.Value;
        }

        return default;
    }

    public void Add(TKey key, TValue value)
    {
        lock (_lock)
        {
            if (_cache.TryGetValue(key, out var node))
            {
                // If the key exists, remove the existing node from the linked list to update it later.
                _cacheItemsOrder.Remove(node);
            }
            else if (_cache.Count >= _capacity)
            {
                // If the cache has reached its capacity, remove the least recently used item.
                var lastNode = _cacheItemsOrder.Last;
                if (lastNode != null)
                {
                    _cache.TryRemove(lastNode.Value.Key, out _);
                    _cacheItemsOrder.RemoveLast();
                }
            }

            // Add a new node to the front of the linked list to mark it as most recently used.
            var cacheItem = new CacheItem<TKey, TValue>(key, value);
            var newNode = new LinkedListNode<CacheItem<TKey, TValue>>(cacheItem);
            _cache[key] = newNode;
            _cacheItemsOrder.AddFirst(newNode);
        }
    }
}
