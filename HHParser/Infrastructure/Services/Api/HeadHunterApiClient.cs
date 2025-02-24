using HHParser.Domain.Models;
using HHParser.Application.Interfaces;
using HHParser.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using HHParser.Infrastructure.Services.Ex;
using System.Runtime.CompilerServices;
using HHParser.Infrastructure.Configuration.Constants;
using AutoMapper;
using System.Collections.Concurrent;
using Polly;
using HHParser.Application.Interfaces.Progress;
using HHParser.Domain.Models.Vacancies;
using Microsoft.AspNetCore.WebUtilities;
using HHParser.Application.Services;

namespace HHParser.Infrastructure.Services.Api
{
    /// <summary>
    /// Handles communication with the HH.ru API, including retrieving data such as specialization groups and professional roles.
    /// Caches the results using a separate caching service.
    /// </summary>
    public class HeadHunterApiClient : IHeadHunterApiClient
    {
        #region Dependency Injection Fields
        private readonly HttpClient _client;
        private readonly ILogger<HeadHunterApiClient> _logger;
        private readonly IMapper _mapper;
        private readonly ICustomProgressBarService _progressService;
        private readonly ICachingService _cachingService;
        #endregion

        #region Endpoint Fields
        private readonly string _specializationsUrl;
        private readonly string _professionalRolesUrl;
        private readonly string _vacanciesUrl;
        private readonly string _vacancyDetailTemplate;
        #endregion

        public HeadHunterApiClient(HttpClient client,
            IOptions<HHApiSettings> options,
            ILogger<HeadHunterApiClient> logger,
            IMapper mapper,
            ICustomProgressBarService progressService,
            ICachingService cachingService)
        {
            _client = client;
            _mapper = mapper;
            _logger = logger;
            _progressService = progressService;
            _cachingService = cachingService;

            var settings = options.Value;
            if (string.IsNullOrWhiteSpace(settings.BaseUrl))
            {
                _logger.LogError("BaseUrl is not set in HHApiSettings.");
                throw new ArgumentException("BaseUrl is not set in HHApiSettings.");
            }

            if (!_client.DefaultRequestHeaders.Contains("User-Agent"))
            {
                _client.DefaultRequestHeaders.Add("User-Agent", "HH-User-Agent");
            }

            _client.BaseAddress = new Uri(settings.BaseUrl);
            _specializationsUrl = settings.SpecializationsPath;
            _professionalRolesUrl = settings.ProfessionalRolesPath;
            _vacanciesUrl = settings.VacanciesUrl;
            _vacancyDetailTemplate = settings.VacancyDetailTemplate;
        }

        /// <summary>
        /// Processes vacancies by retrieving, enriching, and exporting them.
        /// </summary>
        /// <param name="parameters">The query parameters for retrieving vacancies.</param>
        /// <param name="exporter">The data exporter used to export enriched vacancies.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ProcessVacanciesAsync(Dictionary<string, string> parameters, IDataExporter exporter, CancellationToken cancellationToken = default)
        {
            var vacancies = await GetVacanciesAsync(parameters, cancellationToken);
            var enrichedVacancies = await EnrichVacanciesAsync(vacancies, cancellationToken);
            exporter.ExportVacancies(enrichedVacancies, ExportFileNameConstants.VacanciesFileName);
        }

        private async Task<List<VacancySummary>> GetVacanciesAsync(Dictionary<string, string> baseParameters, CancellationToken cancellationToken = default)
        {
            var allVacancies = new ConcurrentBag<VacancySummary>();
            var semaphore = new SemaphoreSlim(HhApiConstants.MaxConcurrentRequests);
            var retryPolicy = PollyPolicyFactory.CreatePolicyWrap<VacancyListResponse>(_logger, nameof(GetVacanciesAsync));

            await _progressService.StartAsync(HhApiConstants.TotalPages, async updater =>
            {
                var tasks = Enumerable.Range(0, HhApiConstants.TotalPages)
                    .Select(async page =>
                    {
                        await ExecuteWithDelayAndSemaphoreAsync(semaphore, async () =>
                        {
                            var parameters = new Dictionary<string, string>(baseParameters)
                            {
                                [HhApiParameterConstants.Page] = page.ToString()
                            };

                            string queryUrl = QueryHelpers.AddQueryString(_vacanciesUrl, parameters);
                            var context = new Context();
                            context[PollyContextKeys.Page] = page;

                            try
                            {
                                var response = await retryPolicy.ExecuteAsync(
                                    (ctx, ct) => GetDataAsObjectAsync<VacancyListResponse>(queryUrl, ct),
                                    context,
                                    cancellationToken);

                                if (response.Items != null)
                                {
                                    foreach (var item in response.Items)
                                    {
                                        allVacancies.Add(item);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Failed to retrieve vacancies for page {Page} after multiple attempts", page);
                            }
                        }, cancellationToken);
                        updater.Increment(HhApiConstants.ProgressIncrementPerItem);
                    });
                await Task.WhenAll(tasks);
            });

            return allVacancies.ToList();
        }

        private async Task<List<EnrichedVacancy>> EnrichVacanciesAsync(List<VacancySummary> vacancies, CancellationToken cancellationToken)
        {
            var enrichedVacancies = new ConcurrentBag<EnrichedVacancy>();
            var semaphore = new SemaphoreSlim(HhApiConstants.MaxConcurrentRequests);
            var retryPolicy = PollyPolicyFactory.CreatePolicyWrap<VacancyDetail>(_logger, nameof(EnrichVacanciesAsync));

            await _progressService.StartAsync(vacancies.Count, async updater =>
            {
                var tasks = vacancies.Select(async vacancy =>
                {
                    await ExecuteWithDelayAndSemaphoreAsync(semaphore, async () =>
                    {
                        string detailUrl = string.Format(_vacancyDetailTemplate, vacancy.Id);

                        var context = new Context();
                        context[PollyContextKeys.VacancyId] = vacancy.Id;

                        VacancyDetail details = null;
                        try
                        {
                            details = await retryPolicy.ExecuteAsync(
                                ctx => GetDataAsObjectAsync<VacancyDetail>(detailUrl, cancellationToken),
                                context);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to retrieve vacancy details for vacancy {VacancyId} after multiple attempts", vacancy.Id);
                        }

                        var enriched = _mapper.Map<EnrichedVacancy>(vacancy);
                        if (details != null)
                        {
                            enriched.Description = details?.Description;
                            enriched.KeySkills = details?.KeySkills?.Select(ks => ks.Name).ToList() ?? new List<string>();
                        }

                        enrichedVacancies.Add(enriched);
                    }, cancellationToken);
                    updater.Increment(HhApiConstants.ProgressIncrementPerItem);
                });
                await Task.WhenAll(tasks);
            });

            return enrichedVacancies.ToList();
        }

        public async Task<List<SpecializationGroup>> GetSpecializationGroupsAsync(CancellationToken cancellationToken = default)
        {
            return await _cachingService.GetOrAddAsync(
                CacheConstants.SpecializationsCacheKey,
                async () => await GetDataAsObjectAsync<List<SpecializationGroup>>(_specializationsUrl, cancellationToken),
                CacheConstants.CacheDuration) ?? new List<SpecializationGroup>();
        }

        public async Task<List<ProfessionalRolesCategory>> GetProfessionalRolesGroupsAsync(CancellationToken cancellationToken = default)
        {
            var response = await _cachingService.GetOrAddAsync(
                CacheConstants.ProfessionalRolesCacheKey,
                async () => await GetDataAsObjectAsync<ProfessionalRolesResponse>(_professionalRolesUrl, cancellationToken),
                CacheConstants.CacheDuration);

            return response?.Categories ?? new List<ProfessionalRolesCategory>();
        }

        private async Task<T> GetDataAsObjectAsync<T>(string endPoint, CancellationToken cancellationToken, [CallerMemberName] string callerMethod = "")
        {
            try
            {
                var response = await _client.GetAsync(endPoint, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Unsuccessful response: {StatusCode} while requesting {Url} in {CallerMethod}",
                        response.StatusCode, endPoint, callerMethod);
                    throw new ApiRequestException($"Unsuccessful response from server: {response.StatusCode}");
                }
                var data = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken).ConfigureAwait(false);
                if (EqualityComparer<T>.Default.Equals(data, default))
                {
                    throw new ApiRequestException("Empty response received from API");
                }
                return data;

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
        /// Executes an operation with a random delay within the context of a specified semaphore.
        /// </summary>
        /// <param name="semaphore">The semaphore to limit concurrent requests.</param>
        /// <param name="operation">The operation to execute.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        private static async Task ExecuteWithDelayAndSemaphoreAsync(SemaphoreSlim semaphore, Func<Task> operation, CancellationToken cancellationToken)
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                int delayMs = Random.Shared.Next((int)HhApiConstants.MinRequestDelay.TotalMilliseconds, (int)HhApiConstants.MaxRequestDelay.TotalMilliseconds);
                await Task.Delay(delayMs, cancellationToken);
                await operation();
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
