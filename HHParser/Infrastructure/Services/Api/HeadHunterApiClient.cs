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
using HHParser.Domain.Models.Vacancies;
using Microsoft.AspNetCore.WebUtilities;
using Spectre.Console;
using HHParser.Infrastructure.Services.Csv;
using AutoMapper;

namespace HHParser.Infrastructure.Services.Api
{
    /// <summary>
    /// Handles communication with the HH.ru API, including retrieving data such as specialization groups and professional roles.
    /// Caches the results to improve performance by reducing the number of API requests.
    /// </summary>
    public class HeadHunterApiClient : IHeadHunterApiClient
    {
        #region DI fields
        private readonly HttpClient _client;
        private readonly ILogger<HeadHunterApiClient> _logger;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;

        #endregion

        #region Endpoint fields
        private readonly string _specializationsUrl;
        private readonly string _professionalRolesUrl;
        private readonly string _vacanciesUrl;
        #endregion

        /// <summary>
        /// Initializes a new instance of the HHApiClient class.
        /// </summary>
        /// <param name="client">The HTTP client to use for making API requests.</param>
        /// <param name="options">Settings for the HH API, including the base URL and paths for specializations and professional roles.</param>
        /// <param name="logger">Logger for logging API-related activities and errors.</param>
        /// <param name="cache">In-memory cache used to store API responses to avoid redundant requests.</param>
        public HeadHunterApiClient(HttpClient client,
            IOptions<HHApiSettings> options,
            ILogger<HeadHunterApiClient> logger,
            IMemoryCache cache,
            IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;

            var settings = options.Value;

            if (string.IsNullOrWhiteSpace(settings.BaseUrl))
            {
                _logger.LogError("BaseUrl is not set in HHApiSettings.");
                throw new ArgumentException("BaseUrl is not set in HHApiSettings.");
            }

            // Добавляем обязательный заголовок User-Agent
            if (!_client.DefaultRequestHeaders.Contains("User-Agent"))
            {
                _client.DefaultRequestHeaders.Add("User-Agent", "HH-User-Agent");
            }

            _client.BaseAddress = new Uri(settings.BaseUrl);

            _specializationsUrl = settings.SpecializationsPath;
            _professionalRolesUrl = settings.ProfessionalRolesPath;
            _vacanciesUrl = settings.VacanciesUrl;
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
        /// Основной метод, который выполняет:
        /// 1. Получение базовых вакансий (20 страниц),
        /// 2. Обогащение каждой вакансии дополнительными данными (например, key_skills),
        /// 3. Сохранение итогового датасета в CSV файл.
        /// </summary>
        public async Task ProcessVacanciesAsync(Dictionary<string, string> baseParameters, CancellationToken cancellationToken = default)
        {
            List<VacancySummary> vacancies = await GetVacanciesAsync(baseParameters, cancellationToken);

            // 2. Обходим вакансии для получения дополнительных данных и формируем EnrichedVacancy
            List<EnrichedVacancy> enrichedVacancies = await EnrichVacanciesAsync(vacancies, cancellationToken);

            // 3. Сохраняем полученные данные в CSV (например, в файл "merged_vacancies.csv")
            CsvFileService.SaveVacanciesToCsv(enrichedVacancies, "merged_vacancies.csv");
        }

        /// <summary>
        /// Получает базовые данные вакансий с API hh.ru по заданным параметрам.
        /// Собирать будем данные за 20 страниц.
        /// </summary>
        public async Task<List<VacancySummary>> GetVacanciesAsync(Dictionary<string, string> baseParameters, CancellationToken cancellationToken = default)
        {
            var allVacancies = new List<VacancySummary>();
            int totalPages = 20;

            await AnsiConsole.Progress().StartAsync(async ctx =>
            {
                var progressTask = ctx.AddTask("[green]Загрузка страниц вакансий...[/]", maxValue: totalPages);

                for (int page = 0; page < totalPages; page++)
                {
                    // Создаём копию параметров и указываем номер страницы
                    var parameters = new Dictionary<string, string>(baseParameters)
                    {
                        ["page"] = page.ToString()
                    };

                    // Перед каждым запросом ждём 2 секунды (ограничение API)
                    await Task.Delay(2000, cancellationToken);

                    // Формируем URL с параметрами запроса
                    var queryUrl = QueryHelpers.AddQueryString(_vacanciesUrl, parameters);
                    _logger.LogInformation("Запрос вакансий для страницы {Page} по URL: {Url}", page, queryUrl);

                    try
                    {
                        // Предполагается, что API возвращает объект типа VacancyListResponse с полем Items
                        var response = await _client.GetFromJsonAsync<VacancyListResponse>(queryUrl, cancellationToken);
                        if (response?.Items != null)
                        {
                            allVacancies.AddRange(response.Items);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Ошибка при загрузке страницы {Page}", page);
                        AnsiConsole.MarkupLine($"[red]Ошибка при загрузке страницы {page}[/]");
                        break;
                    }

                    progressTask.Increment(1);
                }
            });

            return allVacancies;
        }

        /// <summary>
        /// Обходит список вакансий и для каждой получает подробную информацию (например, key_skills).
        /// Результатом является список объектов EnrichedVacancy.
        /// </summary>
        private async Task<List<EnrichedVacancy>> EnrichVacanciesAsync(List<VacancySummary> vacancies, CancellationToken cancellationToken)
        {
            var enrichedList = new List<EnrichedVacancy>();
            int totalVacancies = vacancies.Count;

            await AnsiConsole.Progress().StartAsync(async ctx =>
            {
                var progressTask = ctx.AddTask("[green]Обогащение вакансий...[/]", maxValue: totalVacancies);

                foreach (var vacancy in vacancies)
                {
                    Console.WriteLine($"Вакансия: {vacancy.Id}");
                    // Небольшая задержка между запросами (например, 1 секунда)
                    await Task.Delay(2000, cancellationToken);

                    // Получаем подробности вакансии по её ID
                    VacancyDetail details = await GetVacancyDetailsAsync(vacancy.Id, cancellationToken);

                    // Маппинг базовых данных
                    var enriched = _mapper.Map<EnrichedVacancy>(vacancy);

                    // Если нужно объединить данные из VacancyDetail (например, KeySkills)
                    if (details != null && details.KeySkills != null)
                    {
                        enriched.KeySkills = details.KeySkills.Select(ks => ks.Name).ToList();
                    }

                    //enrichedList.Add(enriched);
                    //progressTask.Increment(1);
                    //// Формируем объект EnrichedVacancy, объединяя базовые данные и дополнительные (ключевые навыки)
                    //var enriched = new EnrichedVacancy(vacancy)
                    //{
                    //    KeySkills = details?.KeySkills?.Select(ks => ks.Name).ToList() ?? new List<string>()
                    //};


                    enrichedList.Add(enriched);
                    progressTask.Increment(1);
                }
            });

            return enrichedList;
        }

        /// <summary>
        /// Получает подробную информацию по конкретной вакансии (например, ключевые навыки).
        /// URL формируется как _vacanciesUrl/{vacancyId}
        /// </summary>
        public async Task<VacancyDetail> GetVacancyDetailsAsync(string vacancyId, CancellationToken cancellationToken)
        {
            string detailUrl = $"{_vacanciesUrl}/{vacancyId}";

            try
            {
                var response = await _client.GetAsync(detailUrl, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка при получении деталей вакансии {VacancyId}: {StatusCode}", vacancyId, response.StatusCode);
                    return null;
                }
                var details = await response.Content.ReadFromJsonAsync<VacancyDetail>(cancellationToken: cancellationToken);
                return details;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе деталей вакансии {VacancyId}", vacancyId);
                return null;
            }
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
                return result ?? [];
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
