using HHParser.Application.Interfaces.Progress;

namespace HHParser.Application.Services.Progress
{
    public class CustomProgressBarService : ICustomProgressBarService
    {
        public async Task StartAsync(double total, Func<IProgressUpdater, Task> action, string description = "")
        {
            var updater = new ConsoleProgressUpdater(description, total);
            await action(updater);
            // После завершения очищаем строку
            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
        }
    }
}
