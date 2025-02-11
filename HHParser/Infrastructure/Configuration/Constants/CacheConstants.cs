namespace HHParser.Infrastructure.Configuration.Constants
{
    /// <summary>
    /// This static class holds constants related to caching configuration,
    /// including cache duration and cache keys for different data.
    /// </summary>
    public static class CacheConstants
    {
        /// <summary>
        /// The duration for which cached data will be stored, for example, 24 hours.
        /// This value determines how long the cache will be considered valid before it is refreshed.
        /// </summary>
        public static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);

        /// <summary>
        /// The key used for caching specialization data.
        /// This key helps identify the cache entry for specialization-related data.
        /// </summary>
        public const string SpecializationsCacheKey = "specializations";

        /// <summary>
        /// The key used for caching professional roles data.
        /// This key helps identify the cache entry for professional roles-related data.
        /// </summary>
        public const string ProfessionalRolesCacheKey = "professionalRoles";
    }
}
