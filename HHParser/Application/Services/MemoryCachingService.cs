using HHParser.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace HHParser.Application.Services
{
    /// <summary>
    /// Provides an implementation of the caching service using in-memory caching.
    /// </summary>
    /// <remarks>
    /// This service uses <see cref="IMemoryCache"/> to store and retrieve data.
    /// It is typically used to improve performance by caching the results of expensive operations.
    public class MemoryCachingService(IMemoryCache cache) : ICachingService
    {
        private readonly IMemoryCache _cache = cache;

        /// <summary>
        /// Retrieves a cached value for the specified key, or executes the provided function to fetch and cache the value.
        /// </summary>
        /// <typeparam name="T">The type of the value to cache.</typeparam>
        /// <param name="cacheKey">The unique key identifying the cached value.</param>
        /// <param name="fetchFunc">
        /// An asynchronous function that retrieves the data if it is not found in the cache.
        /// </param>
        /// <param name="expiration">
        /// The time span after which the cached entry should expire.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation, with the cached or fetched data as its result.
        /// </returns>
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
