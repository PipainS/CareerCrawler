using HHParser.Application.Interfaces.Progress;

namespace HHParser.Application.Services.Progress
{
    /// <summary>
    /// Provides an implementation of <see cref="IProgressUpdater"/> that updates progress on the console.
    /// </summary>
    public class ConsoleProgressUpdater : IProgressUpdater
    {
        private readonly string _description;
        private readonly double _total;
        private double _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleProgressUpdater"/> class.
        /// </summary>
        /// <param name="description">A description of the operation.</param>
        /// <param name="total">The total number of progress steps.</param>
        public ConsoleProgressUpdater(string description, double total)
        {
            _description = description;
            _total = total;
            _current = 0;
            Render();
        }

        /// <summary>
        /// Increments the current progress by the specified amount.
        /// </summary>
        /// <param name="amount">The amount to increment (typically 1 per step).</param>
        public void Increment(double amount)
        {
            _current += amount;
            Render();
        }

        /// <summary>
        /// Renders the progress bar on the console.
        /// </summary>
        private void Render()
        {
            double percentage = Math.Min(100, (_current / _total) * 100);
            // Optionally, add a more sophisticated visualization using symbols.
            int barWidth = 30; // width of the progress bar
            int progressBlocks = (int)((percentage / 100) * barWidth);
            string bar = new string('█', progressBlocks).PadRight(barWidth, '-');

            // Construct the progress string.
            string progressText = $"{_description} [{bar}] {percentage:0}%";
            // Update the line (the "\r" carriage return returns the cursor to the beginning of the line).
            Console.Write("\r" + progressText);
        }
    }
}
