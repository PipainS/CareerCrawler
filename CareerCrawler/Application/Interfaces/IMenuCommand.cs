using HHParser.Domain.Enums;

namespace HHParser.Application.Interfaces
{
    /// <summary>
    /// Represents a command associated with a specific menu option.
    /// </summary>
    public interface IMenuCommand
    {
        /// <summary>
        /// Gets the menu option that corresponds to this command.
        /// </summary>
        MainMenuOption Option { get; }

        /// <summary>
        /// Executes the command from main menu asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to cancel the execution of the command.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
