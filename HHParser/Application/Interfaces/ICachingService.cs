namespace HHParser.Application.Interfaces
{
    public interface ICachingService
    {
        /// <summary>
        /// Возвращает данные из кеша по ключу или выполняет функцию получения данных,
        /// кеширует результат и возвращает его.
        /// </summary>
        Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> fetchFunc, TimeSpan expiration);
    }

}
