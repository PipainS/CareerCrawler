namespace HHParser.Domain.Models
{
    /// <summary>
    /// Represents a single specialization with its ID, name, and an optional laboring status.
    /// This is used to map the specialization data from the API response.
    /// </summary>
    public class Specialization
    {
        /// <summary>
        /// The unique identifier for the specialization.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The name of the specialization.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// A flag indicating if the specialization is laboring (true/false).
        /// </summary>
        public bool? Laboring { get; set; }
    }
}
