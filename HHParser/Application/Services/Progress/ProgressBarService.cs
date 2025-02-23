using HHParser.Application.Interfaces.Progress;
using Spectre.Console;

namespace HHParser.Application.Services.Progress
{
    /// <summary>
    /// Сервис для отображения прогресс-бара, реализованный на базе Spectre.Console.
    /// </summary>
    public class ProgressBarService : IProgressBarService
    {
        public async Task<T> StartAsync<T>(Func<IProgressContext, Task<T>> action)
        {
            return await AnsiConsole.Progress().StartAsync(async context =>
            {
                var wrappedContext = new SpectreProgressContextWrapper(context);
                return await action(wrappedContext);
            });
        }
    }
}
