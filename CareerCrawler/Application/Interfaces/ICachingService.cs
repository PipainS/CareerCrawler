namespace HHParser.Application.Interfaces
{
    /// <summary>
    /// Provides caching functionality for retrieving data with a fallback data retrieval method.
    /// </summary>
    /// <remarks>
    /// This interface defines a method for getting a value from the cache if it exists,
    /// or executing a specified asynchronous function to retrieve and cache the data if not.
    /// </remarks>
    public interface ICachingService
    {
        /// <summary>
        /// Retrieves a cached value for the specified key, or adds it to the cache if it does not exist.
        /// </summary>
        /// <typeparam name="T">The type of the object to cache.</typeparam>
        /// <param name="cacheKey">The unique key identifying the cached value.</param>
        /// <param name="fetchFunc">
        /// An asynchronous function that fetches the data if it's not already cached.
        /// </param>
        /// <param name="expiration">
        /// The duration after which the cached data should expire.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation, with the cached or newly fetched data as its result.
        /// </returns>
        Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> fetchFunc, TimeSpan expiration);
    }
}
