namespace HHParser.Application.Interfaces.Progress
{
    public interface ICustomProgressBarService
    {
        /// <summary>
        /// Запускает асинхронную операцию с отображением прогресс-бара, обновляемого в одной строке.
        /// </summary>
        /// <param name="description">Описание операции (будет выведено в начале строки).</param>
        /// <param name="total">Общее количество шагов.</param>
        /// <param name="action">Делегат, получающий объект для обновления прогресса.</param>
        Task StartAsync(double total, Func<IProgressUpdater, Task> action, string description = "");
    }

}
