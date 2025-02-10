using HHParser.Application.Interfaces;
using HHParser.Domain.Enums;
using HHParser.Presentation.Views;
using Microsoft.Extensions.Logging;

namespace HHParser.Application.Services.MenuService
{
    public class ConsoleMenuService : IMenuService
    {
        private readonly IConsoleView _view;
        private readonly IEnumerable<IMenuCommand> _commands;
        private readonly ILogger<ConsoleMenuService> _logger;

        public ConsoleMenuService(IConsoleView view, IEnumerable<IMenuCommand> commands, ILogger<ConsoleMenuService> logger)
        {
            _view = view;
            _commands = commands;
            _logger = logger;
        }

        public async Task ShowMainMenuAsync(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _view.ShowMainMenu();
                var choice = Console.ReadLine();

                if (!int.TryParse(choice, out int optionValue) ||
                    !Enum.IsDefined(typeof(MainMenuOption), optionValue))
                {
                    _view.ShowError("Некорректный выбор");
                    continue;
                }

                var menuOption = (MainMenuOption)optionValue;

                if (menuOption == MainMenuOption.Exit)
                {
                    _logger.LogInformation("Пользователь выбрал выход.");
                    return;
                }

                var command = GetCommand(menuOption);
                if (command is not null)
                {
                    _logger.LogInformation("Выполнение команды для опции {Option}.", menuOption);
                    await command.ExecuteAsync(cancellationToken);
                }
                else
                {
                    _view.ShowError("Выбранная функция не реализована.");
                }
            }
        }

        private IMenuCommand? GetCommand(MainMenuOption option)
        {
            foreach (var cmd in _commands)
            {
                if (cmd.Option == option)
                    return cmd;
            }
            return null;
        }
    }
}
