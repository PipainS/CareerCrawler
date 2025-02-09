using HHParser.Domain.Models;
using HHParser.Application.Interfaces;
using HHParser.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using HHParser.Infrastructure.Services.Ex;

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
                throw new ArgumentException("Не задан BaseUrl в настройках HHApiSettings.");
            }

            _specializationsUrl = $"{settings.BaseUrl}/specializations";
            _professionalRolesUrl = $"{settings.BaseUrl}/professional_roles";
        }

        public async Task<List<SpecializationGroup>> GetSpecializationGroupsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Получение специализаций с URL: {Url}", _specializationsUrl);
                using var response = await _client.GetAsync(_specializationsUrl, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка при запросе специализаций: {StatusCode}", response.StatusCode);
                    throw new ApiRequestException($"Ошибка при получении специализаций: {response.StatusCode}");
                }

                using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var result = await JsonSerializer.DeserializeAsync<List<SpecializationGroup>>(stream, cancellationToken: cancellationToken);

                return result ?? new List<SpecializationGroup>();
            }
            catch (Exception ex)
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
                using var response = await _client.GetAsync(_professionalRolesUrl, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка при запросе профессиональных ролей: {StatusCode}", response.StatusCode);
                    throw new ApiRequestException($"Ошибка при получении профессиональных ролей: {response.StatusCode}");
                }

                using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var result = await JsonSerializer.DeserializeAsync<List<ProfessionalRolesGroup>>(stream, cancellationToken: cancellationToken);

                return result ?? new List<ProfessionalRolesGroup>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении профессиональных ролей.");
                throw new ApiRequestException("Ошибка при получении данных с API проф. ролей.", ex);
            }
        }
    }
}
