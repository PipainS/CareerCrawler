using HHParser.Application.Interfaces;
using HHParser.Domain.Enums;
using HHParser.Infrastructure.Services.Ex;

namespace HHParser.Application.Commands
{
    /// <summary>
    /// Command responsible for fetching and displaying specialization groups from the hh.ru API.
    /// </summary>
    public class SpecializationsCommand : IMenuCommand
    {
        /// <summary>
        /// Gets the menu option associated with this command.
        /// </summary>
        public MainMenuOption Option => MainMenuOption.Specializations;

        private readonly IHHService _hhService;
        private readonly IConsoleView _view;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecializationsCommand"/> class.
        /// </summary>
        /// <param name="hhService">The service used to retrieve specialization groups from the hh.ru API.</param>
        /// <param name="view">The console view used to display specialization groups and error messages.</param>
        public SpecializationsCommand(IHHService hhService, IConsoleView view)
        {
            _hhService = hhService;
            _view = view;
        }

        /// <summary>
        /// Executes the command to retrieve specialization groups and display them using the console view.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ApiRequestException">Thrown when the API call fails.</exception>
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve specialization groups using the hh.ru API.
                var groups = await _hhService.GetSpecializationGroupsAsync(cancellationToken);
                _view.ShowSpecializations(groups);
            }
            catch (ApiRequestException ex)
            {
                _view.ShowError($"API error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Unexpected error: {ex.Message}");
            }
        }
    }
}
