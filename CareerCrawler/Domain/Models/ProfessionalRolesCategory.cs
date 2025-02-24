namespace HHParser.Domain.Models
{
    /// <summary>
    /// Represents a category of professional roles.
    /// A category groups together similar professional roles (e.g., "Автомобильный бизнес").
    /// </summary>
    public class ProfessionalRolesCategory
    {
        /// <summary>
        /// The unique identifier for the category (e.g., "19" for "Автомобильный бизнес").
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The name of the professional roles category (e.g., "Автомобильный бизнес").
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// A list of professional roles within this category.
        /// These roles are specific job positions within the category (e.g., "Автомойщик", "Менеджер по продажам").
        /// </summary>
        public List<ProfessionalRole>? Roles { get; set; }
    }
}
