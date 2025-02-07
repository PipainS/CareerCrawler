using HHParser.Models;
using Newtonsoft.Json;

namespace HHParser.Services
{
    public class HHApiService
    {
        private readonly HttpClient _client;
        private const string ApiUrl = "https://api.hh.ru/specializations";

        public HHApiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<SpecializationGroup>> GetSpecializationGroupsAsync()
        {
            try
            {
                var response = await _client.GetStringAsync(ApiUrl);

                var groups = JsonConvert.DeserializeObject<List<SpecializationGroup>>(response);

                return groups;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ошибка при получении данных: {ex.Message}");
                throw;
            }
        }
    }
}
