namespace HHParser.Domain.Models
{
    /// <summary>
    /// Represents the response containing the list of professional role categories.
    /// This is used to map the API response for professional roles.
    /// </summary>
    public class ProfessionalRolesResponse
    {
        /// <summary>
        /// A list of categories that organize different professional roles.
        /// Each category contains a list of professional roles (e.g., "Автомобильный бизнес", "Административный персонал").
        /// </summary>
        public List<ProfessionalRolesCategory>? Categories { get; set; }
    }
}
