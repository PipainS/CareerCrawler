using HHParser.Application.Interfaces.Progress;
using Spectre.Console;

namespace HHParser.Application.Services.Progress
{
    /// <summary>
    /// Обёртка для отдельной задачи в прогресс-баре.
    /// </summary>
    public class SpectreProgressTaskWrapper : IProgressTask
    {
        private readonly ProgressTask _progressTask;

        public SpectreProgressTaskWrapper(ProgressTask progressTask)
        {
            _progressTask = progressTask;
        }

        public void Increment(double amount)
        {
            _progressTask.Increment(amount);
        }
    }
}
