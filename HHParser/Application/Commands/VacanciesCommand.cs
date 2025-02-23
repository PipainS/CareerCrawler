using HHParser.Application.Interfaces;
using HHParser.Application.Services.CommonClasses;
using HHParser.Domain.Enums;
using HHParser.Domain.Models;
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
                var vacancyParams = PromptForVacancySearchParameters();
                var queryParameters = QueryParameterHelper.ToDictionary(vacancyParams);


                // Запрашиваем формат экспорта у пользователя с использованием enum
                var exportFormat = AnsiConsole.Prompt(
                    new SelectionPrompt<ExportFormat>()
                        .Title("Выберите формат сохранения данных:")
                        .AddChoices(Enum.GetValues(typeof(ExportFormat)).Cast<ExportFormat>())
                );

                // Получаем нужный экспортер через фабрику
                IDataExporter exporter = DataExporterFactory.GetExporter(exportFormat);

                // Передаём константу с именем файла (например, для вакансий)
                await _hhService.ProcessVacanciesAsync(queryParameters, exporter, cancellationToken);
            }
            catch (NotSupportedException ex)
            {
                _view.ShowError($"Не реализован формат: {ex.Message}");
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

        private static VacancySearchParameters PromptForVacancySearchParameters()
        {
            var parameters = new VacancySearchParameters();
            parameters.Text = AnsiConsole.Ask<string>("Введите ключевое слово для поиска ([green]text[/]):");
            parameters.PerPage = AnsiConsole.Ask<int>("Введите количество вакансий на странице ([green]per_page[/]) (по умолчанию 10):", 10);
            return parameters;
        }
    }
}
