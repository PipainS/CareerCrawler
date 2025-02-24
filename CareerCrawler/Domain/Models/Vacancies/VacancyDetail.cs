using System.Text.Json.Serialization;

namespace HHParser.Domain.Models.Vacancies
{
    public class VacancyDetail : VacancySummary
    {
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("key_skills")]
        public List<KeySkillInfo>? KeySkills { get; set; }
    }
}
