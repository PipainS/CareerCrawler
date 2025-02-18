using System.Text.Json.Serialization;
using HHParser.Domain.Attributes;
using Newtonsoft.Json;

namespace HHParser.Domain.Models.Vacancies
{
    /// <summary>
    /// Ответ API с кратким списком вакансий.
    /// </summary>
    public class VacancyListResponse
    {
        [JsonProperty("items")]
        public List<VacancySummary> Items { get; set; }

        [JsonProperty("found")]
        public int Found { get; set; }

        [JsonProperty("pages")]
        public int Pages { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("per_page")]
        public int PerPage { get; set; }

        [JsonProperty("alternate_url")]
        public string AlternateUrl { get; set; }
    }

    /// <summary>
    /// Краткая информация о вакансии.
    /// </summary>
    public class VacancySummary
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Название вакансии.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Флаг премиум-вакансии.
        /// </summary>
        [JsonProperty("premium")]
        public bool Premium { get; set; }

        /// <summary>
        /// Зарплатные данные.
        /// </summary>
        [JsonProperty("salary")]
        public SalaryInfo Salary { get; set; }

        /// <summary>
        /// Область (город) вакансии – используется только имя.
        /// </summary>
        [JsonProperty("area")]
        public AreaInfo Area { get; set; }

        /// <summary>
        /// Работодатель – используется только имя.
        /// </summary>
        [JsonProperty("employer")]
        public EmployerInfo Employer { get; set; }

        /// <summary>
        /// Дата публикации вакансии.
        /// </summary>
        [JsonPropertyName("published_at")]
        [System.Text.Json.Serialization.JsonConverter(typeof(CustomDateTimeOffsetConverter))]

        public DateTimeOffset PublishedAt { get; set; }

        /// <summary>
        /// Дата создания вакансии.
        /// </summary>
        [JsonPropertyName("created_at")]
        [System.Text.Json.Serialization.JsonConverter(typeof(CustomDateTimeOffsetConverter))]

        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Опыт работы – используется только имя.
        /// </summary>
        [JsonProperty("experience")]
        public ExperienceInfo Experience { get; set; }

        /// <summary>
        /// Занятость – используется только имя.
        /// </summary>
        [JsonProperty("employment")]
        public EmploymentInfo Employment { get; set; }

        /// <summary>
        /// График работы – используется только имя.
        /// </summary>
        [JsonProperty("schedule")]
        public ScheduleInfo Schedule { get; set; }

        /// <summary>
        /// Флаг рекламной вакансии.
        /// </summary>
        [JsonPropertyName("is_adv_vacancy")]
        public bool IsAdvVacancy { get; set; }
    }

    /// <summary>
    /// Подробная информация о вакансии, расширяющая краткую и включающая описание и ключевые навыки.
    /// </summary>
    public class VacancyDetail : VacancySummary
    {
        /// <summary>
        /// Полное описание вакансии.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Список ключевых навыков вакансии.
        /// </summary>
        [JsonPropertyName("key_skills")]
        public List<KeySkillInfo> KeySkills { get; set; }
    }

    /// <summary>
    /// Зарплатные данные вакансии.
    /// </summary>
    public class SalaryInfo
    {
        [JsonProperty("from")]
        public int? From { get; set; }

        [JsonProperty("to")]
        public int? To { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("gross")]
        public bool Gross { get; set; }
    }

    /// <summary>
    /// Информация об области – используется только имя.
    /// </summary>
    public class AreaInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Информация о работодателе – используется только имя.
    /// </summary>
    public class EmployerInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Опыт работы – используется только имя.
    /// </summary>
    public class ExperienceInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Информация о занятости – используется только имя.
    /// </summary>
    public class EmploymentInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Информация о графике работы – используется только имя.
    /// </summary>
    public class ScheduleInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Ключевой навык вакансии.
    /// </summary>
    public class KeySkillInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class EnrichedVacancy : VacancySummary
    {
        /// <summary>
        /// Список ключевых навыков (например, полученных из VacancyDetail).
        /// </summary>
        public List<string> KeySkills { get; set; } = new List<string>();

        // Пустой конструктор, если потребуется
        public EnrichedVacancy() { }

        // Конструктор для копирования свойств из VacancySummary
        public EnrichedVacancy(VacancySummary summary)
        {
            // Копирование всех полей из базового объекта VacancySummary
            Id = summary.Id;
            Name = summary.Name;
            Premium = summary.Premium;
            Salary = summary.Salary;
            Area = summary.Area;
            Employer = summary.Employer;
            PublishedAt = summary.PublishedAt;
            CreatedAt = summary.CreatedAt;
            Experience = summary.Experience;
            Employment = summary.Employment;
            Schedule = summary.Schedule;
            IsAdvVacancy = summary.IsAdvVacancy;
        }
    }
}

