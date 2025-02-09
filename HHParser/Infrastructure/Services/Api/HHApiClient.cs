using HHParser.Domain.Models;
using HHParser.Application.Interfaces;
using HHParser.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using HHParser.Infrastructure.Services.Ex;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HHParser.Infrastructure.Services.Api
{
    public class HHApiClient : IHHService
    {
        private readonly HttpClient _client;
        private readonly ILogger<HHApiClient> _logger;

        private readonly string _specializationsUrl;
        private readonly string _professionalRolesUrl;

        public HHApiClient(HttpClient client, IOptions<HHApiSettings> options, ILogger<HHApiClient> logger)
        {
            _client = client;
            _logger = logger;

            var settings = options.Value;

            if (string.IsNullOrWhiteSpace(settings.BaseUrl))
            {
                _logger.LogError("BaseUrl не задан в настройках HHApiSettings.");
                throw new System.ArgumentException("Не задан BaseUrl в настройках HHApiSettings.");
            }

            _specializationsUrl = $"{settings.BaseUrl}/specializations";
            _professionalRolesUrl = $"{settings.BaseUrl}/professional_roles";
        }

        public async Task<List<SpecializationGroup>> GetSpecializationGroupsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Получение специализаций с URL: {Url}", _specializationsUrl);
                var result = await _client.GetFromJsonAsync<List<SpecializationGroup>>(_specializationsUrl, cancellationToken);
                return result ?? new List<SpecializationGroup>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Ошибка HTTP при запросе специализаций: {Url}", _specializationsUrl);
                throw new ApiRequestException("Ошибка при получении специализаций.", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении специализаций.");
                throw new ApiRequestException("Ошибка при получении данных с API специализаций.", ex);
            }
        }

        public async Task<List<ProfessionalRolesGroup>> GetProfessionalRolesGroupsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Получение ролей с URL: {Url}", _professionalRolesUrl);
                var result = await _client.GetFromJsonAsync<List<ProfessionalRolesGroup>>(_professionalRolesUrl, cancellationToken);
                return result ?? new List<ProfessionalRolesGroup>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Ошибка HTTP при запросе профессиональных ролей: {Url}", _professionalRolesUrl);
                throw new ApiRequestException("Ошибка при получении профессиональных ролей.", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении профессиональных ролей.");
                throw new ApiRequestException("Ошибка при получении данных с API проф. ролей.", ex);
            }
        }
    }
}
