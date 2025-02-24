namespace HHParser.Domain.Models.Vacancies
{
    public class EnrichedVacancy : VacancySummary
    {
        public List<string> KeySkills { get; set; } = new List<string>();

        public string? Description { get; set; }

        public EnrichedVacancy() { }
    }
}
