namespace HHParser.Domain.Models
{
    public class Specialization
    {
        public required string Id { get; set; }

        public required string Name { get; set; }

        public string? Laboring { get; set; }
    }
}
