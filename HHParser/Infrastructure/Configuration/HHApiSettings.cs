namespace HHParser.Infrastructure.Configuration
{
    /// <summary>
    /// Represents the configuration settings for the HH API.
    /// These values are typically retrieved from appsettings.json to build URLs for API requests.
    /// </summary>
    public class HHApiSettings
    {
        /// <summary>
        /// The base URL of the HH API, e.g., https://api.hh.ru/.
        /// This will be used to build the full API URLs.
        /// </summary>
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// The path for specializations endpoint, which will be appended to the base URL.
        /// Example value: "specializations"
        /// </summary>
        public string SpecializationsPath { get; set; } = string.Empty;

        /// <summary>
        /// The path for professional roles endpoint, which will be appended to the base URL.
        /// Example value: "professional_roles"
        /// </summary>
        public string ProfessionalRolesPath { get; set; } = string.Empty;

        public string VacanciesUrl { get; set; } = string.Empty;

        public string VacancyDetailTemplate { get; set; } = string.Empty;

    }
}
