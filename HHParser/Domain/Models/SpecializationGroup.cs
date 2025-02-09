namespace HHParser.Domain.Models
{
    public class SpecializationGroup
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public List<Specialization>? Specializations { get; set; }
    }
}
