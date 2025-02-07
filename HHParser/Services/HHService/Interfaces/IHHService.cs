using HHParser.Models;

namespace HHParser.Services.HHService.Interfaces
{
    public interface IHHService
    {
        Task<List<SpecializationGroup>> GetSpecializationGroupsAsync();
    }
}
