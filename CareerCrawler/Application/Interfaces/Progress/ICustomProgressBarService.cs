using HHParser.Infrastructure.Configuration.Constants;

namespace HHParser.Application.Interfaces.Progress
{
    /// <summary>
    /// Provides functionality to start an asynchronous operation with a console progress bar updated in a single line.
    /// </summary>
    public interface ICustomProgressBarService
    {
        /// <summary>
        /// Starts an asynchronous operation with a progress bar.
        /// </summary>
        /// <param name="total">The total number of steps.</param>
        /// <param name="action">A delegate that receives an object for updating progress.</param>
        /// <param name="description">A description of the operation (displayed at the beginning of the line).</param>
        Task StartAsync(double total, Func<IProgressUpdater, Task> action, string description = ProgressBarConstants.DefaultLoadingText);
    }
}
