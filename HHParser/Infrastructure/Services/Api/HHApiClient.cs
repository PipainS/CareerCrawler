using HHParser.Domain.Models;
using HHParser.Application.Interfaces;
using HHParser.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace HHParser.Infrastructure.Services.Api
{
    public class HHApiClient : IHHService
    {
        private readonly HttpClient _client;
        private readonly string _specializationsUrl;
        private readonly string _professionalRolesUrl;

        public HHApiClient(HttpClient client, IOptions<HHApiSettings> options)
        {
            _client = client;
            var settings = options.Value;

            if (string.IsNullOrWhiteSpace(settings.BaseUrl))
            {
                throw new ArgumentException("Не задан BaseUrl в настройках HHApiSettings.");
            }

            _specializationsUrl = $"{settings.BaseUrl}/specializations";
            _professionalRolesUrl = $"{settings.BaseUrl}/professional_roles";
        }

        public async Task<List<SpecializationGroup>> GetSpecializationGroupsAsync()
        {
            var response = await _client.GetStringAsync(_specializationsUrl);
            return JsonConvert.DeserializeObject<List<SpecializationGroup>>(response)
                   ?? new List<SpecializationGroup>();
        }

        public async Task<List<ProfessionalRolesGroup>> GetProfessionalRolesGroupsAsync()
        {
            var response = await _client.GetStringAsync(_professionalRolesUrl);
            return JsonConvert.DeserializeObject<List<ProfessionalRolesGroup>>(response)
                   ?? new List<ProfessionalRolesGroup>();
        }
    }
}
