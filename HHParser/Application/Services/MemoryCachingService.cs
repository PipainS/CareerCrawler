using HHParser.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace HHParser.Application.Services
{
    public class MemoryCachingService : ICachingService
    {
        private readonly IMemoryCache _cache;

        public MemoryCachingService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> fetchFunc, TimeSpan expiration)
        {
            if (_cache.TryGetValue(cacheKey, out T cachedValue))
            {
                return cachedValue;
            }

            T result = await fetchFunc();
            _cache.Set(cacheKey, result, expiration);
            return result;
        }
    }
}
