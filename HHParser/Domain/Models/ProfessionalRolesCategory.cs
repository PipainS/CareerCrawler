namespace HHParser.Domain.Models
{
    public class ProfessionalRolesCategory
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public List<ProfessionalRole>? Roles { get; set; }
    }
}
