namespace HHParser.Application.Interfaces.Progress
{
    /// <summary>
    /// Defines the method for updating progress.
    /// </summary>
    public interface IProgressUpdater
    {
        /// <summary>
        /// Increments the current progress value.
        /// </summary>
        /// <param name="amount">The amount to increase the progress (typically 1 per step).</param>
        void Increment(double amount);
    }
}
