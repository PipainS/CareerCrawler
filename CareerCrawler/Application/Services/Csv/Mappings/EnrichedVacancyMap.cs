using CsvHelper.Configuration;
using HHParser.Domain.Models.Vacancies;

namespace HHParser.Application.Services.Csv.Mappings
{
    public class EnrichedVacancyMap : ClassMap<EnrichedVacancy>
    {
        public EnrichedVacancyMap()
        {
            // Указываем, какие свойства выводить и как называть колонки в CSV

            // Из VacancySummary
            Map(v => v.Id).Name("VacancyId");
            Map(v => v.Name).Name("VacancyName");
            Map(v => v.Premium).Name("IsPremium");
            Map(v => v.PublishedAt).Name("PublishedAt");
            Map(v => v.CreatedAt).Name("CreatedAt");
            Map(v => v.IsAdvVacancy).Name("IsAdvVacancy");

            // SalaryInfo (вложенный объект)
            Map(v => v.Salary.From).Name("SalaryFrom");
            Map(v => v.Salary.To).Name("SalaryTo");
            Map(v => v.Salary.Currency).Name("SalaryCurrency");
            Map(v => v.Salary.Gross).Name("SalaryGross");

            // AreaInfo
            Map(v => v.Area.Name).Name("Area");

            // EmployerInfo
            Map(v => v.Employer.Name).Name("Employer");

            // ExperienceInfo
            Map(v => v.Experience.Name).Name("Experience");

            // EmploymentInfo
            Map(v => v.Employment.Name).Name("Employment");

            // ScheduleInfo
            Map(v => v.Schedule.Name).Name("ScheduleName");

            Map(v => v.Description).Name("Description");


            // KeySkills — список строк
            Map(v => v.KeySkills)
                .Name("KeySkills")
                .Convert(row => string.Join(", ", row.Value.KeySkills ?? new List<string>()));
        }
    }
}
