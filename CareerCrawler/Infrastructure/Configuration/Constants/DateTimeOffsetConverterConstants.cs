namespace HHParser.Infrastructure.Configuration.Constants
{
    /// <summary>
    /// Contains constants used in the <see cref="CustomDateTimeOffsetConverter"/> for parsing DateTimeOffset values.
    /// </summary>
    public static class DateTimeOffsetConverterConstants
    {
        /// <summary>
        /// The expected length of the timezone offset part in the datetime string.
        /// For example, "+0300" has a length of 5.
        /// </summary>
        public const int TimeZoneOffsetLength = 5;

        /// <summary>
        /// The number of characters representing the hour part in the timezone offset.
        /// For example, in "+0300", the hour part "+03" has a length of 3.
        /// </summary>
        public const int TimeZoneHourPartLength = 3;
    }
}
