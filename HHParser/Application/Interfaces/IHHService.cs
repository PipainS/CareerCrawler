using HHParser.Domain.Models;

namespace HHParser.Application.Interfaces
{
    /// <summary>
    /// Defines a service for interacting with the hh.ru API to retrieve job-related data.
    /// </summary>
    public interface IHHService
    {
        /// <summary>
        /// Retrieves a list of specialization groups from the hh.ru API.
        /// </summary>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation, with a result of a list of <see cref="SpecializationGroup"/> objects.
        /// </returns>
        Task<List<SpecializationGroup>> GetSpecializationGroupsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a list of professional roles categories from the hh.ru API.
        /// </summary>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation, with a result of a list of <see cref="ProfessionalRolesCategory"/> objects.
        /// </returns>
        Task<List<ProfessionalRolesCategory>> GetProfessionalRolesGroupsAsync(CancellationToken cancellationToken = default);
    }
}
