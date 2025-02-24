using HHParser.Infrastructure.Configuration.Constants;
using Microsoft.Extensions.Logging;
using Polly;

namespace HHParser.Application.Services
{
    /// <summary>
    /// Factory class for creating wrapped Polly policies that combine circuit breaker and retry strategies.
    /// The circuit breaker acts as the outer layer, while the retry policy is nested within.
    /// </summary>
    public static class PollyPolicyFactory
    {
        /// <summary>
        /// Creates a wrapped policy (circuit breaker + retry) for asynchronous operations.
        /// The circuit breaker acts as the outer layer, and retry is the inner layer.
        /// </summary>
        /// <typeparam name="T">The type of the operation result.</typeparam>
        /// <param name="logger">The logger for outputting messages.</param>
        /// <param name="operationName">The name of the operation (used for logging).</param>
        /// <param name="retryCount">The number of retry attempts.</param>
        /// <param name="circuitBreakerThreshold">The number of exceptions allowed before the circuit breaker triggers.</param>
        /// <param name="breakDurationSeconds">The duration (in seconds) for which the circuit breaker remains open.</param>
        /// <returns>A combined Polly asynchronous policy.</returns>
        public static IAsyncPolicy<T> CreatePolicyWrap<T>(
            ILogger logger,
            string operationName,
            int retryCount = PollyPolicyDefaults.RetryCount,
            int circuitBreakerThreshold = PollyPolicyDefaults.CircuitBreakerThreshold,
            int breakDurationSeconds = PollyPolicyDefaults.BreakDurationSeconds)
        {
            // Retry policy with exponential back-off.
            var retryPolicy = Policy<T>
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount,
                    attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: (outcome, timeSpan, retryAttempt, context) =>
                    {
                        var contextInfo = GetContextInfo(context);
                        logger.LogWarning("Error during {OperationName}, attempt {RetryAttempt}. {ContextInfo} Error: {ErrorMessage}",
                            operationName, retryAttempt, contextInfo, outcome.Exception?.Message);
                    });

            // Circuit breaker policy that triggers after a specified number of exceptions.
            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: circuitBreakerThreshold,
                    durationOfBreak: TimeSpan.FromSeconds(breakDurationSeconds),
                    onBreak: (exception, timespan) =>
                    {
                        logger.LogWarning("Circuit breaker triggered for {OperationName}. Breaking for {BreakDuration} seconds. Exception: {ExceptionMessage}",
                            operationName, breakDurationSeconds, exception.Message);
                    },
                    onReset: () =>
                    {
                        logger.LogInformation("Circuit breaker reset for {OperationName}", operationName);
                    })
                .AsAsyncPolicy<T>();

            // Combine policies: outer layer is the circuit breaker, inner layer is the retry policy.
            return Policy.WrapAsync(circuitBreakerPolicy, retryPolicy);
        }

        /// <summary>
        /// Helper method to form context information from the Polly execution context.
        /// </summary>
        /// <param name="context">The Polly execution context.</param>
        /// <returns>
        /// A string representing additional context information.
        /// For example, returns "Page: {value}" if the key "Page" is found,
        /// or "VacancyId: {value}" if the key "VacancyId" is found; otherwise, returns an empty string.
        /// </returns>
        private static string GetContextInfo(Context context)
        {
            if (context.TryGetValue(PollyContextKeys.Page, out var page))
                return $"Page: {page}";
            if (context.TryGetValue(PollyContextKeys.VacancyId, out var vacancyId))
                return $"VacancyId: {vacancyId}";
            return string.Empty;
        }
    }
}
