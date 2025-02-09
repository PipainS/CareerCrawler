using HHParser.Application.Interfaces;
using HHParser.Domain.Models;
using HHParser.Presentation.Views;
using HHParser.Domain.Enums;
using HHParser.Infrastructure.Services.Ex;

namespace HHParser.Application.Commands
{
    public class SpecializationsCommand : IMenuCommand
    {
        public MainMenuOption Option => MainMenuOption.Specializations;

        private readonly IHHService _hhService;
        private readonly ConsoleView _view;

        public SpecializationsCommand(IHHService hhService, ConsoleView view)
        {
            _hhService = hhService;
            _view = view;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Получаем специализации с учётом cancellationToken
                var groups = await _hhService.GetSpecializationGroupsAsync(cancellationToken);
                _view.ShowSpecializations(groups);

                //var inputIds = _view.GetUserInputIds();
                //var (selectedGroups, selectedSpecializations) = ProcessUserInput(groups, inputIds);
                //_view.ShowSelectionResults(selectedGroups, selectedSpecializations);
            }
            catch (ApiRequestException ex)
            {
                _view.ShowError($"Ошибка API: {ex.Message}");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Неожиданная ошибка: {ex.Message}");
            }
        }

        private (List<string>, List<string>) ProcessUserInput(List<SpecializationGroup> groups, HashSet<string> inputIds)
        {
            var selectedGroups = new List<string>();
            var selectedSpecializations = new List<string>();

            foreach (var group in groups)
            {
                if (inputIds.Contains(group.Id))
                {
                    selectedGroups.Add(group.Name);
                }

                var matchingSpecs = group.Specializations?
                    .Where(spec => inputIds.Contains(spec.Id))
                    .Select(spec => spec.Name) ?? Enumerable.Empty<string>();

                selectedSpecializations.AddRange(matchingSpecs);
            }

            return (selectedGroups, selectedSpecializations);
        }
    }
}
