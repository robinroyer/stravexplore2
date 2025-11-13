using Microsoft.Extensions.Caching.Memory;

namespace StravaConsumer.API.Services;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<CacheService> _logger;

    public CacheService(IMemoryCache memoryCache, ILogger<CacheService> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public T? Get<T>(string key)
    {
        _memoryCache.TryGetValue(key, out T? value);
        return value;
    }

    public void Set<T>(string key, T value, TimeSpan? expiration = null)
    {
        var cacheOptions = new MemoryCacheEntryOptions();

        if (expiration.HasValue)
        {
            cacheOptions.SetAbsoluteExpiration(expiration.Value);
        }
        else
        {
            cacheOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
        }

        _memoryCache.Set(key, value, cacheOptions);
        _logger.LogDebug("Cached item with key: {Key}", key);
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
        _logger.LogDebug("Removed cache item with key: {Key}", key);
    }

    public bool Exists(string key)
    {
        return _memoryCache.TryGetValue(key, out _);
    }
}
