using Microsoft.Extensions.Caching.Memory;
using UINavigator.Services;

public class MemCache : IMemCache
{
    private readonly IMemoryCache _memoryCache;

    public MemCache(IMemoryCache memoryCache) =>
        _memoryCache = memoryCache;

    public T? Get<T>(string key)
    {
        T? val = default;
        if (!_memoryCache.TryGetValue(key, out val))
        {
            return default;
        }

        return val;
    }

    public void Set<T>(string key, T value)
    {
        if (!_memoryCache.TryGetValue(key, out T? _))
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                .SetSize(1024);

            _memoryCache.Set(key, value, cacheEntryOptions);
        }
    }
}
