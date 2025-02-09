namespace HHParser.Domain.Models
{
    public class ProfessionalRole
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public bool AcceptIncompleteResumes { get; set; }
        public bool IsDefault { get; set; }
        public bool SelectDeprecated { get; set; }
        public bool SearchDeprecated { get; set; }
    }
}
