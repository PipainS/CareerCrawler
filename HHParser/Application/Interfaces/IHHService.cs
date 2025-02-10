using HHParser.Domain.Models;

namespace HHParser.Application.Interfaces
{
    public interface IHHService
    {
        Task<List<SpecializationGroup>> GetSpecializationGroupsAsync(CancellationToken cancellationToken = default);
        Task<List<ProfessionalRolesCategory>> GetProfessionalRolesGroupsAsync(CancellationToken cancellationToken = default);
    }
}
