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
    }
}
