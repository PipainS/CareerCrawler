using HHParser.Application.Interfaces;
using HHParser.Domain.Enums;
using HHParser.Infrastructure.Services.Ex;
using Spectre.Console;

namespace HHParser.Application.Commands
{
    public class VacanciesCommand : IMenuCommand
    {
        public MainMenuOption Option => MainMenuOption.VacancySearch;


        private readonly IHeadHunterApiClient _hhService;
        private readonly IConsoleView _view;

        public VacanciesCommand(IHeadHunterApiClient hhService, IConsoleView view)
        {
            _hhService = hhService;
            _view = view;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                var parameters = PromptForVacancySearchParameters();

                // Получаем базовый список вакансий в виде VacancySummary (новые модели)
                //var vacancies = await _hhService.GetVacanciesAsync(parameters, cancellationToken);
                //_view.ShowVacancies(vacancies);
                await _hhService.ProcessVacanciesAsync(parameters, cancellationToken);  
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
        /// Запрашивает у пользователя параметры поиска вакансий.
        /// </summary>
        private Dictionary<string, string> PromptForVacancySearchParameters()
        {
            var parameters = new Dictionary<string, string>();

            var text = AnsiConsole.Ask<string>("Введите ключевое слово для поиска ([green]text[/]):");
            if (!string.IsNullOrWhiteSpace(text))
            {
                parameters.Add("text", text);
            }

            // Количество вакансий на странице (по умолчанию 10, максимум 100)
            var perPage = AnsiConsole.Ask<int>("Введите количество вакансий на странице ([green]per_page[/]) (по умолчанию 10):", 10);
            parameters.Add("per_page", perPage.ToString());

            return parameters;
        }
    }
}
