using HHParser.Application.Interfaces;
using HHParser.Application.Services.CommonClasses;
using HHParser.Domain.Enums;
using HHParser.Domain.Models;
using HHParser.Infrastructure.Services.Ex;
using Spectre.Console;

namespace HHParser.Application.Commands
{
    /// <summary>
    /// Command class that handles vacancy search operations.
    /// </summary>
    /// <remarks>
    /// This class is invoked when the "VacancySearch" option is selected from the main menu.
    /// It prompts the user for vacancy search parameters, handles export format selection, and delegates
    /// the processing of vacancies to the HeadHunter API client.
    /// </remarks>
    public class VacanciesCommand : IMenuCommand
    {
        /// <summary>
        /// Gets the main menu option associated with this command.
        /// </summary>
        public MainMenuOption Option => MainMenuOption.VacancySearch;

        private readonly IHeadHunterApiClient _hhService;
        private readonly IConsoleView _view;

        /// <summary>
        /// Initializes a new instance of the <see cref="VacanciesCommand"/> class.
        /// </summary>
        /// <param name="hhService">The HeadHunter API client used to process vacancies.</param>
        /// <param name="view">The console view used to display messages and errors.</param>
        public VacanciesCommand(IHeadHunterApiClient hhService, IConsoleView view)
        {
            _hhService = hhService;
            _view = view;
        }

        /// <summary>
        /// Executes the vacancy search command.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous execution of the command.</returns>
        /// <remarks>
        /// This method prompts the user for vacancy search parameters, selects an export format,
        /// and calls the HeadHunter API client to process vacancies. It also handles exceptions such as unsupported
        /// export formats and API request errors.
        /// </remarks>
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                var vacancyParams = PromptForVacancySearchParameters();
                var queryParameters = QueryParameterHelper.ToDictionary(vacancyParams);

                // Prompts the user to select the export format using an enum.
                var exportFormat = AnsiConsole.Prompt(
                    new SelectionPrompt<ExportFormat>()
                        .Title("Select the format for saving data:")
                        .AddChoices(Enum.GetValues(typeof(ExportFormat)).Cast<ExportFormat>())
                );

                // Retrieves the appropriate exporter via a factory.
                IDataExporter exporter = DataExporterFactory.GetExporter(exportFormat);

                // Processes vacancies using the HeadHunter API client, passing the query parameters and exporter.
                await _hhService.ProcessVacanciesAsync(queryParameters, exporter, cancellationToken);
            }
            catch (NotSupportedException ex)
            {
                _view.ShowError($"The format is not implemented: {ex.Message}");
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

        /// <summary>
        /// Prompts the user to input vacancy search parameters.
        /// </summary>
        /// <returns>A <see cref="VacancySearchParameters"/> object containing the user's search criteria.</returns>
        private static VacancySearchParameters PromptForVacancySearchParameters()
        {
            var parameters = new VacancySearchParameters();
            parameters.Text = AnsiConsole.Ask<string>("Enter the search keyword ([green]text[/]):");
            parameters.PerPage = AnsiConsole.Ask<int>("Enter the number of vacancies on the page ([green]per_page[/]) (default is 10):", 10);
            return parameters;
        }
    }
}
