namespace HHParser.Domain.Models
{
    /// <summary>
    /// Represents a group of specializations with their ID, name, and associated specializations.
    /// This is used to map the specialization group data from the API response.
    /// </summary>
    public class SpecializationGroup
    {
        /// <summary>
        /// The unique identifier for the specialization group.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The name of the specialization group.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// A list of specializations associated with this group.
        /// This represents the actual job roles in that group.
        /// </summary>
        public List<Specialization>? Specializations { get; set; }
    }
}
