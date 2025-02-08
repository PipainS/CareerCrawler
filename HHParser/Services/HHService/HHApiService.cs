using HHParser.Configuration;
using HHParser.Models;
using HHParser.Services.HHService.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace HHParser.Services.HHService
{
    public class HHApiService : IHHService
    {
        private readonly HttpClient _client;

        public string ApiUrl { get; private set; }

        public string ProfAreaApiUrl { get; private set; }

        public HHApiService(HttpClient client, IConfiguration configuration)
        {
            _client = client;

            ApiUrl = configuration.GetSection(HHApiSettings.SectionName)["BaseUrl"] + "/specializations";
            ProfAreaApiUrl = configuration.GetSection(HHApiSettings.SectionName)["BaseUrl"] + "/professional_roles";

        }

        public async Task<List<SpecializationGroup>> GetSpecializationGroupsAsync()
        {
            try
            {
                var response = await _client.GetStringAsync(ApiUrl);
                return JsonConvert.DeserializeObject<List<SpecializationGroup>>(response) ?? [];

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ошибка при получении данных с API",
                                               ex);
            }
        }

        public async Task<List<ProfessionalRolesGroup>> GetProfessionalRolesGroupsAsync()
        {
            try
            {
                var response = await _client.GetStringAsync(ProfAreaApiUrl);
                return JsonConvert.DeserializeObject<List<ProfessionalRolesGroup>>(response) ?? [];
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ошибка при получении данных с API",
                                               ex);
            }
        }
    }
}
