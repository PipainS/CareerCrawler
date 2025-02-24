using HHParser.Domain.Attributes;

namespace HHParser.Domain.Models
{
    /// <summary>
    /// Represents the search parameters for querying vacancies.
    /// These parameters are mapped to query string parameters in the API request.
    /// </summary>
    public class VacancySearchParameters
    {
        /// <summary>
        /// The keyword or phrase used for searching vacancies.
        /// This corresponds to the "text" query parameter in the API.
        /// </summary>
        [QueryParameter("text")]
        public string? Text { get; set; } = string.Empty;

        /// <summary>
        /// The number of vacancies to retrieve per page.
        /// This corresponds to the "per_page" query parameter in the API.
        /// Default value is 10.
        /// </summary>
        [QueryParameter("per_page")]
        public int PerPage { get; set; } = 10;
    }
}
