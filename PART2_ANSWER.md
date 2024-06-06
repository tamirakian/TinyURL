# Part 2: Adding Memory Cache Layer


## Approach for Size-Limited Cache

I implemented a Least Recently Used (LRU) cache. In an LRU cache, the least recently used items are discarded first when the cache reaches its maximum capacity.
This approach is well-suited for URL redirection scenarios because it ensures that frequently accessed URLs remain in the cache, reducing the number of database accesses.

**Advantages of LRU Cache:**
- Simple and efficient cache eviction strategy - beneficial for handling bursts of redirections.
- Good balance between hit rate and memory usage.

**Disadvantages of LRU Cache:**
- Storing additional metadata (e.g., cache items order) consumes extra memory.