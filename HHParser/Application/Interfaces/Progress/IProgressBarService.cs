namespace HHParser.Application.Interfaces.Progress
{
    /// <summary>
    /// Интерфейс сервиса для запуска операций с прогресс-баром.
    /// </summary>
    public interface IProgressBarService
    {
        /// <summary>
        /// Запускает операцию с отображением прогресс-бара.
        /// </summary>
        /// <typeparam name="T">Тип результата операции.</typeparam>
        /// <param name="action">
        /// Делегат, получающий контекст прогресс-бара и выполняющий асинхронную операцию.
        /// </param>
        /// <returns>Результат выполнения операции.</returns>
        Task<T> StartAsync<T>(Func<IProgressContext, Task<T>> action);
    }
}