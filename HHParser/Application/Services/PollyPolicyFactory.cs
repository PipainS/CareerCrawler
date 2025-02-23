using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;

namespace HHParser.Application.Services
{
    public static class PollyPolicyFactory
    {
        /// <summary>
        /// Создаёт обёрнутую политику (retry + circuit breaker) для асинхронных операций.
        /// </summary>
        /// <typeparam name="T">Тип результата операции.</typeparam>
        /// <param name="logger">Логгер для вывода сообщений.</param>
        /// <param name="operationName">Имя операции (для логирования).</param>
        /// <param name="retryCount">Количество повторных попыток.</param>
        /// <param name="circuitBreakerThreshold">Количество исключений до срабатывания circuit breaker.</param>
        /// <param name="breakDurationSeconds">Длительность разрыва circuit breaker.</param>
        /// <returns>Скомбинированная политика Polly.</returns>
        public static IAsyncPolicy<T> CreatePolicyWrap<T>(
            ILogger logger,
            string operationName,
            int retryCount = 3,
            int circuitBreakerThreshold = 5,
            int breakDurationSeconds = 30)
        {
            // Retry-политика для generic типа
            var retryPolicy = Policy<T>
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount,
                    attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: (outcome, timeSpan, retryAttempt, context) =>
                    {
                        var contextInfo = context.ContainsKey("Page")
                            ? $"Страница: {context["Page"]}"
                            : context.ContainsKey("VacancyId")
                                ? $"VacancyId: {context["VacancyId"]}"
                                : string.Empty;
                        logger.LogWarning("Ошибка при {OperationName}, попытка {RetryAttempt}. {ContextInfo} Ошибка: {ErrorMessage}",
                            operationName, retryAttempt, contextInfo, outcome.Exception?.Message);
                    });

            // Circuit breaker-политика создаётся как неунифицированная, затем приводится к IAsyncPolicy<T>
            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: circuitBreakerThreshold,
                    durationOfBreak: TimeSpan.FromSeconds(breakDurationSeconds),
                    onBreak: (exception, timespan) =>
                    {
                        logger.LogWarning("Circuit breaker сработал для {OperationName}. Прерывание на {BreakDuration} секунд. Исключение: {ExceptionMessage}",
                            operationName, breakDurationSeconds, exception.Message);
                    },
                    onReset: () =>
                    {
                        logger.LogInformation("Circuit breaker сброшен для {OperationName}", operationName);
                    })
                .AsAsyncPolicy<T>();

            // Объединяем политики: сначала retry, затем circuit breaker
            return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
        }
    }
}
