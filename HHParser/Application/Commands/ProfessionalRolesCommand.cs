using HHParser.Application.Interfaces;
using HHParser.Domain.Enums;
using HHParser.Infrastructure.Services.Ex;
using Microsoft.Extensions.Logging;

namespace HHParser.Application.Commands
{
    /// <summary>
    /// Command responsible for fetching and displaying professional roles categories from the hh.ru API.
    /// </summary>
    public class ProfessionalRolesCommand : IMenuCommand
    {
        /// <summary>
        /// Gets the menu option associated with this command.
        /// </summary>
        public MainMenuOption Option => MainMenuOption.ProfessionalRolesSearch;

        private readonly IConsoleView _consoleView;
        private readonly IHHService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfessionalRolesCommand"/> class.
        /// </summary>
        /// <param name="logger">The logger used to record command execution details and errors.</param>
        /// <param name="consoleView">The console view used to display professional roles and error messages.</param>
        /// <param name="service">The service used to retrieve professional roles categories from the hh.ru API.</param>
        public ProfessionalRolesCommand(ILogger<ProfessionalRolesCommand> logger, IConsoleView consoleView, IHHService service)
        {
            _consoleView = consoleView;
            _service = service;
        }

        /// <summary>
        /// Executes the command to retrieve professional roles categories and display them using the console view.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ApiRequestException">Thrown when the API call fails.</exception>
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve professional roles categories using the hh.ru API.
                var categories = await _service.GetProfessionalRolesGroupsAsync(cancellationToken);
                _consoleView.ShowProfessionalRoles(categories);
            }
            catch (ApiRequestException ex)
            {
                _consoleView.ShowError($"API error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _consoleView.ShowError($"Unexpected error: {ex.Message}");
            }
        }
    }
}
