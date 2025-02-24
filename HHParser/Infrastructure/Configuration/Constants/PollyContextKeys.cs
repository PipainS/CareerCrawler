namespace HHParser.Infrastructure.Configuration.Constants
{
    /// <summary>
    /// Contains constant keys used in the Polly execution context.
    /// These keys allow additional contextual information, such as page number or vacancy ID,
    /// to be passed into Polly policies for enhanced logging, debugging, and handling of retry operations.
    /// </summary>
    public static class PollyContextKeys
    {
        /// <summary>
        /// Key for storing the page number in the Polly context.
        /// </summary>
        public const string Page = "Page";

        /// <summary>
        /// Key for storing the vacancy ID in the Polly context.
        /// </summary>
        public const string VacancyId = "VacancyId";
    }
}
