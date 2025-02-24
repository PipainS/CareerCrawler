namespace HHParser.Infrastructure.Configuration.Constants
{
    /// <summary>
    /// Contains constants for configuring requests to the HH.ru API.
    /// Note:
    /// - The HH.ru API supports a maximum of 20 pages for vacancy retrieval.
    /// - The request delays are configured to help prevent triggering captchas.
    /// </summary>
    public static class HhApiConstants
    {
        /// <summary>
        /// Total number of pages for retrieving vacancies.
        /// The HH.ru API supports a maximum of 20 pages.
        /// </summary>
        public const int TotalPages = 20;

        /// <summary>
        /// Maximum number of concurrent requests allowed.
        /// This limits the load and reduces the risk of triggering rate limits or captchas.
        /// </summary>
        public const int MaxConcurrentRequests = 3;

        /// <summary>
        /// Minimum delay between requests.
        /// This delay is set to avoid triggering captchas.
        /// </summary>
        public static readonly TimeSpan MinRequestDelay = TimeSpan.FromMilliseconds(1500);

        /// <summary>
        /// Maximum delay between requests.
        /// This delay is set to avoid triggering captchas.
        /// </summary>
        public static readonly TimeSpan MaxRequestDelay = TimeSpan.FromMilliseconds(2000);

        /// <summary>
        /// Increment value used for progress updates per processed item.
        /// </summary>
        public const int ProgressIncrementPerItem = 1;
    }
}
