using System.Threading;
using HHParser.Domain.Models;

namespace HHParser.Application.Interfaces
{
    public interface IHHService
    {
        Task<List<SpecializationGroup>> GetSpecializationGroupsAsync(CancellationToken cancellationToken = default);
        Task<List<ProfessionalRolesGroup>> GetProfessionalRolesGroupsAsync(CancellationToken cancellationToken = default);
    }
}
