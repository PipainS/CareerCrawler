using HHParser.Application.Interfaces.Progress;
using HHParser.Infrastructure.Configuration.Constants;

namespace HHParser.Application.Services.Progress
{
    /// <summary>
    /// Provides functionality to execute asynchronous operations with a custom progress bar displayed on the console.
    /// </summary>
    public class CustomProgressBarService : ICustomProgressBarService
    {
        /// <summary>
        /// Starts an asynchronous operation with a progress bar.
        /// </summary>
        /// <param name="total">The total number of steps for the progress.</param>
        /// <param name="action">A delegate that performs the operation and updates the progress.</param>
        /// <param name="description">A description of the operation (displayed at the beginning of the line).</param>
        public async Task StartAsync(double total, Func<IProgressUpdater, Task> action, string description = ProgressBarConstants.DefaultLoadingText)
        {
            var updater = new ConsoleProgressUpdater(description, total);
            await action(updater);
            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
        }
    }
}
