using HHParser.Domain.Models;
using HHParser.Application.Interfaces;
using HHParser.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using HHParser.Infrastructure.Services.Ex;
using System.Runtime.CompilerServices;
using HHParser.Infrastructure.Configuration.Constants;
using Microsoft.Extensions.Caching.Memory;

namespace HHParser.Infrastructure.Services.Api
{
    /// <summary>
    /// Handles communication with the HH.ru API, including retrieving data such as specialization groups and professional roles.
    /// Caches the results to improve performance by reducing the number of API requests.
    /// </summary>
    public class HHApiClient : IHHService
    {
        #region DI fields
        private readonly HttpClient _client;
        private readonly ILogger<HHApiClient> _logger;
        private readonly IMemoryCache _cache;
        #endregion

        #region Endpoint fields
        private readonly string _specializationsUrl;
        private readonly string _professionalRolesUrl;
        #endregion

        /// <summary>
        /// Initializes a new instance of the HHApiClient class.
        /// </summary>
        /// <param name="client">The HTTP client to use for making API requests.</param>
        /// <param name="options">Settings for the HH API, including the base URL and paths for specializations and professional roles.</param>
        /// <param name="logger">Logger for logging API-related activities and errors.</param>
        /// <param name="cache">In-memory cache used to store API responses to avoid redundant requests.</param>
        public HHApiClient(HttpClient client,
            IOptions<HHApiSettings> options,
            ILogger<HHApiClient> logger,
            IMemoryCache cache)
        {
            _client = client;
            _logger = logger;
            _cache = cache;

            var settings = options.Value;

            if (string.IsNullOrWhiteSpace(settings.BaseUrl))
            {
                _logger.LogError("BaseUrl is not set in HHApiSettings.");
                throw new ArgumentException("BaseUrl is not set in HHApiSettings.");
            }

            _client.BaseAddress = new Uri(settings.BaseUrl);

            _specializationsUrl = settings.SpecializationsPath;
            _professionalRolesUrl = settings.ProfessionalRolesPath;
        }

        /// <summary>
        /// Retrieves a list of specialization groups from the HH API, with caching to improve performance.
        /// </summary>
        /// <param name="cancellationToken">A token for cancelling the request if needed.</param>
        /// <returns>A list of specialization groups.</returns>
        public async Task<List<SpecializationGroup>> GetSpecializationGroupsAsync(CancellationToken cancellationToken = default)
        {
            return await GetDataWithCacheAsync<SpecializationGroup>(
                _specializationsUrl,
                CacheConstants.SpecializationsCacheKey,
                cancellationToken);
        }

        /// <summary>
        /// Retrieves a list of professional roles categories from the HH API, with caching to improve performance.
        /// </summary>
        /// <param name="cancellationToken">A token for cancelling the request if needed.</param>
        /// <returns>A list of professional roles categories.</returns>
        public async Task<List<ProfessionalRolesCategory>> GetProfessionalRolesGroupsAsync(CancellationToken cancellationToken = default)
        {
            var response = await GetDataWithCacheAsObjectAsync<ProfessionalRolesResponse>(
                _professionalRolesUrl,
                CacheConstants.ProfessionalRolesCacheKey,
                cancellationToken);

            return response?.Categories ?? [];
        }

        /// <summary>
        /// Retrieves data from the API and caches the result if it's not already cached.
        /// </summary>
        /// <typeparam name="T">The type of data expected from the API response.</typeparam>
        /// <param name="endPoint">The API endpoint to retrieve data from.</param>
        /// <param name="cacheKey">The cache key used to store/retrieve the data from the cache.</param>
        /// <param name="cancellationToken">A token for cancelling the request if needed.</param>
        /// <returns>The cached or retrieved data.</returns>
        private async Task<T?> GetDataWithCacheAsObjectAsync<T>(
            string endPoint,
            string cacheKey,
            CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(cacheKey, out T cachedValue))
            {
                _logger.LogInformation("Cache hit for key {CacheKey}. Using cached data.", cacheKey);
                return cachedValue;
            }

            _logger.LogInformation("Cache miss for key {CacheKey}. Requesting data from URL: {Url}", cacheKey, endPoint);
            var value = await GetDataAsObjectAsync<T>(endPoint, cancellationToken);

            _cache.Set(cacheKey, value, CacheConstants.CacheDuration);
            return value;
        }

        /// <summary>
        /// Fetches data from the API and returns it as an object of type T.
        /// This method is used when the response is expected to be deserialized directly into an object.
        /// </summary>
        /// <typeparam name="T">The type of the object expected in the response.</typeparam>
        /// <param name="endPoint">The API endpoint to retrieve data from.</param>
        /// <param name="cancellationToken">A token for cancelling the request if needed.</param>
        /// <param name="callerMethod">The name of the calling method (used for logging purposes).</param>
        /// <returns>The deserialized data from the API response.</returns>
        private async Task<T?> GetDataAsObjectAsync<T>(
            string endPoint,
            CancellationToken cancellationToken,
            [CallerMemberName] string callerMethod = "")
        {
            try
            {
                _logger.LogInformation("Fetching data from {CallerMethod}: {Url}", callerMethod, endPoint);

                var response = await _client.GetAsync(endPoint, cancellationToken).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Unsuccessful response: {StatusCode} while requesting {Url} in {CallerMethod}",
                        response.StatusCode, endPoint, callerMethod);
                    throw new ApiRequestException($"Unsuccessful response from server: {response.StatusCode}");
                }

                var result = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken)
                             .ConfigureAwait(false);
                return result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error in {CallerMethod} while requesting: {Url}", callerMethod, endPoint);
                throw new ApiRequestException($"Error while fetching data in method {callerMethod}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {CallerMethod} while fetching data", callerMethod);
                throw new ApiRequestException($"Error while fetching data in method {callerMethod}", ex);
            }
        }

        /// <summary>
        /// Retrieves data from the API and caches the result if it's not already cached.
        /// This method is used when the response is expected to be a list of items.
        /// </summary>
        /// <typeparam name="T">The type of the list items in the response.</typeparam>
        /// <param name="endPoint">The API endpoint to retrieve data from.</param>
        /// <param name="cacheKey">The cache key used to store/retrieve the data from the cache.</param>
        /// <param name="cancellationToken">A token for cancelling the request if needed.</param>
        /// <returns>A list of the items retrieved from the API or cache.</returns>
        private async Task<List<T>> GetDataWithCacheAsync<T>(
            string endPoint,
            string cacheKey,
            CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(cacheKey, out List<T> cachedValue))
            {
                _logger.LogInformation("Cache hit for key {CacheKey}. Using cached data.", cacheKey);
                return cachedValue;
            }

            _logger.LogInformation("Cache miss for key {CacheKey}. Requesting data from URL: {Url}", cacheKey, endPoint);
            var value = await GetDataAsync<T>(endPoint, cancellationToken);

            _cache.Set(cacheKey, value, CacheConstants.CacheDuration);
            return value;
        }

        /// <summary>
        /// Fetches a list of items from the API and returns it as a list of type T.
        /// This method is used when the response is expected to be a list.
        /// </summary>
        /// <typeparam name="T">The type of the list items expected in the response.</typeparam>
        /// <param name="endPoint">The API endpoint to retrieve data from.</param>
        /// <param name="cancellationToken">A token for cancelling the request if needed.</param>
        /// <param name="callerMethod">The name of the calling method (used for logging purposes).</param>
        /// <returns>A list of items deserialized from the API response.</returns>
        private async Task<List<T>> GetDataAsync<T>(
            string endPoint,
            CancellationToken cancellationToken,
            [CallerMemberName] string callerMethod = "")
        {
            try
            {
                _logger.LogInformation("Fetching data from {CallerMethod}: {Url}", callerMethod, endPoint);

                var response = await _client.GetAsync(endPoint, cancellationToken).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Unsuccessful response: {StatusCode} while requesting {Url} in {CallerMethod}", response.StatusCode, endPoint, callerMethod);
                    throw new ApiRequestException($"Unsuccessful response from server: {response.StatusCode}");
                }

                var result = await response.Content.ReadFromJsonAsync<List<T>>(cancellationToken: cancellationToken).ConfigureAwait(false);
                return result ?? new List<T>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error in {CallerMethod} while requesting: {Url}", callerMethod, endPoint);
                throw new ApiRequestException($"Error while fetching data in method {callerMethod}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {CallerMethod} while fetching data", callerMethod);
                throw new ApiRequestException($"Error while fetching data in method {callerMethod}", ex);
            }
        }
    }
}
