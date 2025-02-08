using HHParser.Domain.Models;
using HHParser.Application.Interfaces;
using HHParser.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

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

        public async Task<List<SpecializationGroup>> GetSpecializationGroupsAsync()
        {
            try
            {
                _logger.LogInformation("Получение специализаций с URL: {Url}", _specializationsUrl);
                var response = await _client.GetStringAsync(_specializationsUrl);
                var result = JsonConvert.DeserializeObject<List<SpecializationGroup>>(response);
                return result ?? new List<SpecializationGroup>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении специализаций.");
                throw new ApplicationException("Ошибка при получении данных с API специализаций.", ex);
            }
        }

        public async Task<List<ProfessionalRolesGroup>> GetProfessionalRolesGroupsAsync()
        {
            try
            {
                _logger.LogInformation("Получение ролей с URL: {Url}", _specializationsUrl);
                var response = await _client.GetStringAsync(_professionalRolesUrl);
                var result = JsonConvert.DeserializeObject<List<ProfessionalRolesGroup>>(response);
                return result ?? new List<ProfessionalRolesGroup>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении профессиональных ролей.");
                throw new ApplicationException("Ошибка при получении данных с API проф. ролей.", ex);
            }

        }
    }
}
