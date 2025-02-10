using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHParser.Application.Interfaces;
using HHParser.Domain.Enums;
using HHParser.Infrastructure.Services.Ex;
using HHParser.Presentation.Views;
using Microsoft.Extensions.Logging;

namespace HHParser.Application.Commands
{
    public class ProfessionalRolesCommand : IMenuCommand
    {
        public MainMenuOption Option => MainMenuOption.ProfessionalRolesSearch;


        private readonly ILogger<ProfessionalRolesCommand> _logger;
        private readonly ConsoleView _consoleView;
        private readonly IHHService _service;
        public ProfessionalRolesCommand(ILogger<ProfessionalRolesCommand> logger, ConsoleView consoleView, IHHService service)
        {
            _logger = logger;
            _consoleView = consoleView;
            _service = service;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                var categories = await _service.GetProfessionalRolesGroupsAsync(cancellationToken);
                _consoleView.ShowProfessionalRoles(categories);
            }
            catch (ApiRequestException ex)
            {
                _consoleView.ShowError($"Ошибка API: {ex.Message}");
            }
            catch (Exception ex)
            {
                _consoleView.ShowError($"Неожиданная ошибка: {ex.Message}");
            }
        }

    }
}
