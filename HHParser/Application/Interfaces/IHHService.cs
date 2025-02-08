using HHParser.Domain.Models;

namespace HHParser.Application.Interfaces
{
    public interface IHHService
    {
        Task<List<SpecializationGroup>> GetSpecializationGroupsAsync();
        Task<List<ProfessionalRolesGroup>> GetProfessionalRolesGroupsAsync();
    }
}
