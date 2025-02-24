using HHParser.Application.Interfaces;
using HHParser.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace HHParser.Application.Services.MenuService
{
    /// <summary>
    /// Service for displaying and managing the console menu. Handles user input and executes the corresponding commands.
    /// </summary>
    public class ConsoleMenuService : IMenuService
    {
        private readonly IConsoleView _view;
        private readonly IEnumerable<IMenuCommand> _commands;
        private readonly ILogger<ConsoleMenuService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMenuService"/> class.
        /// </summary>
        /// <param name="view">The view used to display the console menu.</param>
        /// <param name="commands">A collection of menu commands that can be executed.</param>
        /// <param name="logger">The logger used to log actions and errors.</param>
        public ConsoleMenuService(IConsoleView view, IEnumerable<IMenuCommand> commands, ILogger<ConsoleMenuService> logger)
        {
            _view = view;
            _commands = commands;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task ShowMainMenuAsync(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _view.ShowMainMenu();
                var choice = Console.ReadLine();

                if (!int.TryParse(choice, out int optionValue) ||
                    !Enum.IsDefined(typeof(MainMenuOption), optionValue))
                {
                    _view.ShowError("Invalid choice");
                    continue;
                }

                var menuOption = (MainMenuOption)optionValue;

                if (menuOption == MainMenuOption.Exit)
                {
                    _logger.LogInformation("User selected exit.");
                    return;
                }

                var command = GetCommand(menuOption);
                if (command is not null)
                {
                    _logger.LogInformation("Executing command for option {Option}.", menuOption);
                    await command.ExecuteAsync(cancellationToken);
                }
                else
                {
                    _view.ShowError("The selected function is not implemented.");
                }
            }
        }

        /// <summary>
        /// Retrieves the command associated with the selected menu option.
        /// </summary>
        /// <param name="option">The menu option for which a command is needed.</param>
        /// <returns>The matching menu command, or null if no matching command is found.</returns>
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
