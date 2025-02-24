namespace HHParser.Infrastructure.Configuration.Constants
{
    /// <summary>
    /// Default configuration values for Polly policies.
    /// </summary>
    public static class PollyPolicyDefaults
    {
        /// <summary>
        /// The default number of retry attempts.
        /// </summary>
        public const int RetryCount = 3;

        /// <summary>
        /// The default number of exceptions allowed before triggering the circuit breaker.
        /// </summary>
        public const int CircuitBreakerThreshold = 5;

        /// <summary>
        /// The default duration (in seconds) for which the circuit breaker remains open.
        /// </summary>
        public const int BreakDurationSeconds = 30;
    }
}
