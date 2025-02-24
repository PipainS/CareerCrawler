using System.Text.Json.Serialization;
using HHParser.Domain.Attributes;

namespace HHParser.Domain.Models.Vacancies
{
    public class VacancySummary
    {
        public required string Id { get; set; }

        public required string Name { get; set; }

        public bool Premium { get; set; }

        public SalaryInfo? Salary { get; set; }

        public AreaInfo? Area { get; set; }

        public EmployerInfo? Employer { get; set; }

        [JsonPropertyName("published_at")]
        [JsonConverter(typeof(CustomDateTimeOffsetConverter))]
        public DateTimeOffset PublishedAt { get; set; }

        [JsonPropertyName("created_at")]
        [JsonConverter(typeof(CustomDateTimeOffsetConverter))]
        public DateTimeOffset CreatedAt { get; set; }

        public ExperienceInfo? Experience { get; set; }

        public EmploymentInfo? Employment { get; set; }

        public ScheduleInfo? Schedule { get; set; }

        public bool IsAdvVacancy { get; set; }
    }
}
