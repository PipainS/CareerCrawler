using HHParser.Application.Interfaces.Progress;
using Spectre.Console;

namespace HHParser.Application.Services.Progress
{
    /// <summary>
    /// Обёртка для контекста прогресс-бара.
    /// </summary>
    public class SpectreProgressContextWrapper : IProgressContext
    {
        private readonly ProgressContext _context;

        public SpectreProgressContextWrapper(ProgressContext context)
        {
            _context = context;
        }

        public IProgressTask AddTask(string description, double maxValue)
        {
            // Параметр autoStart: true позволяет сразу запускать задачу.
            var task = _context.AddTask(description, autoStart: true, maxValue: maxValue);
            return new SpectreProgressTaskWrapper(task);
        }
    }
}
