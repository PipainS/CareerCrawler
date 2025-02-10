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
    public class HHApiClient : IHHService
    {
        private readonly HttpClient _client;
        private readonly ILogger<HHApiClient> _logger;
        private readonly IMemoryCache _cache;

        private readonly string _specializationsUrl;
        private readonly string _professionalRolesUrl;

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
                _logger.LogError("BaseUrl не задан в настройках HHApiSettings.");
                throw new ArgumentException("Не задан BaseUrl в настройках HHApiSettings.");
            }

            _client.BaseAddress = new Uri(settings.BaseUrl);
            _specializationsUrl = settings.SpecializationsPath;
            _professionalRolesUrl = settings.ProfessionalRolesPath;
        }

        public async Task<List<SpecializationGroup>> GetSpecializationGroupsAsync(CancellationToken cancellationToken = default)
        {
            return await GetDataWithCacheAsync<SpecializationGroup>(
                _specializationsUrl,
                CacheConstants.SpecializationsCacheKey,
                cancellationToken);
        }

        public async Task<List<ProfessionalRolesCategory>> GetProfessionalRolesGroupsAsync(CancellationToken cancellationToken = default)
        {
            // Получаем объект ProfessionalRolesResponse, а не список.
            var response = await GetDataWithCacheAsObjectAsync<ProfessionalRolesResponse>(
                _professionalRolesUrl,
                CacheConstants.ProfessionalRolesCacheKey,
                cancellationToken);

            return response?.Categories ?? new List<ProfessionalRolesCategory>();
        }


        private async Task<T?> GetDataWithCacheAsObjectAsync<T>(
            string endPoint,
            string cacheKey,
            CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(cacheKey, out T cachedValue))
            {
                _logger.LogInformation("Найден кэш по ключу {CacheKey}. Используем кэшированные данные.", cacheKey);
                return cachedValue;
            }

            _logger.LogInformation("Кэш по ключу {CacheKey} не найден. Запрос данных по URL: {Url}", cacheKey, endPoint);
            var value = await GetDataAsObjectAsync<T>(endPoint, cancellationToken);
            _cache.Set(cacheKey, value, CacheConstants.CacheDuration);
            return value;
        }


        private async Task<T?> GetDataAsObjectAsync<T>(
            string endPoint,
            CancellationToken cancellationToken,
            [CallerMemberName] string callerMethod = "")
        {
            try
            {
                _logger.LogInformation("Получение данных из {CallerMethod}: {Url}", callerMethod, endPoint);
                var response = await _client.GetAsync(endPoint, cancellationToken).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Неуспешный ответ: {StatusCode} при запросе {Url} в {CallerMethod}",
                        response.StatusCode, endPoint, callerMethod);
                    throw new ApiRequestException($"Неуспешный ответ от сервера: {response.StatusCode}");
                }

                // Здесь десериализуем не в список, а в T напрямую.
                var result = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken)
                             .ConfigureAwait(false);
                return result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Ошибка HTTP в {CallerMethod} при запросе: {Url}", callerMethod, endPoint);
                throw new ApiRequestException($"Ошибка при получении данных в методе {callerMethod}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в {CallerMethod} при получении данных", callerMethod);
                throw new ApiRequestException($"Ошибка при получении данных в методе {callerMethod}", ex);
            }
        }


        private async Task<List<T>> GetDataWithCacheAsync<T>(
            string endPoint,
            string cacheKey,
            CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(cacheKey, out List<T> cachedValue))
            {
                _logger.LogInformation("Найден кэш по ключу {CacheKey}. Используем кэшированные данные.", cacheKey);
                return cachedValue;
            }

            _logger.LogInformation("Кэш по ключу {CacheKey} не найден. Запрос данных по URL: {Url}", cacheKey, endPoint);
            var value = await GetDataAsync<T>(endPoint, cancellationToken);
            _cache.Set(cacheKey, value, CacheConstants.CacheDuration);
            return value;
        }

        private async Task<List<T>> GetDataAsync<T>(
            string endPoint,
            CancellationToken cancellationToken,
            [CallerMemberName] string callerMethod = "")
        {
            try
            {
                _logger.LogInformation("Получение данных из {CallerMethod}: {Url}", callerMethod, endPoint);
                var response = await _client.GetAsync(endPoint, cancellationToken).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Неуспешный ответ: {StatusCode} при запросе {Url} в {CallerMethod}", response.StatusCode, endPoint, callerMethod);
                    throw new ApiRequestException($"Неуспешный ответ от сервера: {response.StatusCode}");
                }
                var result = await response.Content.ReadFromJsonAsync<List<T>>(cancellationToken: cancellationToken).ConfigureAwait(false);
                return result ?? new List<T>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Ошибка HTTP в {CallerMethod} при запросе: {Url}", callerMethod, endPoint);
                throw new ApiRequestException($"Ошибка при получении данных в методе {callerMethod}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в {CallerMethod} при получении данных", callerMethod);
                throw new ApiRequestException($"Ошибка при получении данных в методе {callerMethod}", ex);
            }
        }
    }
}
