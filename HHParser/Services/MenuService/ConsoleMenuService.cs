using HHParser.Models;
using HHParser.Services.HHService.Interfaces;
using HHParser.Services.MenuService.Interfaces;
using HHParser.Views;

namespace HHParser.Services.MenuService
{
    public class ConsoleMenuService : IMenuService
    {
        private readonly IHHService _hhService;
        private readonly ConsoleView _view;

        public ConsoleMenuService(IHHService hhService, ConsoleView view)
        {
            _hhService = hhService;
            _view = view;
        }

        public async Task ShowMainMenuAsync()
        {
            while (true)
            {
                _view.ShowMainMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await HandleSpecializationsMenu();
                        break;
                    case "2":
                        // Другие пункты меню
                        break;
                    case "0":
                        return;
                    default:
                        _view.ShowError("Некорректный выбор");
                        break;
                }
            }
        }

        private async Task HandleSpecializationsMenu()
        {
            var groups = await _hhService.GetSpecializationGroupsAsync();
            _view.ShowSpecializations(groups);

            var inputIds = _view.GetUserInputIds();
            var (selectedGroups, selectedSpecializations) = ProcessUserInput(groups, inputIds);
            _view.ShowSelectionResults(selectedGroups, selectedSpecializations);
        }

        private async Task HandleProfessionalRolesMenu()
        {
            var profRolesGroup = await _hhService.GetProfessionalRolesGroupsAsync();

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
