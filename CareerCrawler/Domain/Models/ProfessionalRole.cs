namespace HHParser.Domain.Models
{
    /// <summary>
    /// Represents a professional role (job position) in a given category.
    /// A role includes various properties like the role's name, ID, and other specific flags.
    /// </summary>
    public class ProfessionalRole
    {
        /// <summary>
        /// The unique identifier for the professional role (e.g., "4" for "Автомойщик").
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The name of the professional role (e.g., "Автомойщик").
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// A flag indicating if the role accepts incomplete resumes. 
        /// If true, the role allows resumes that are not fully complete.
        /// </summary>
        public bool AcceptIncompleteResumes { get; set; }

        /// <summary>
        /// A flag indicating if this role is the default role for its category.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// A flag indicating if this role is deprecated and should not be selected.
        /// </summary>
        public bool SelectDeprecated { get; set; }

        /// <summary>
        /// A flag indicating if this role is deprecated in search results.
        /// </summary>
        public bool SearchDeprecated { get; set; }
    }
}
