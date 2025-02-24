using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using HHParser.Infrastructure.Configuration.Constants;

namespace HHParser.Domain.Attributes
{
    /// <summary>
    /// Custom JSON converter for <see cref="DateTimeOffset"/> that handles non-standard timezone formats.
    /// </summary>
    /// <remarks>
    /// Some API responses provide datetime strings with timezone offsets in a non-standard format without a colon,
    /// for example: "2025-02-10T13:46:53+0300" instead of the ISO 8601 compliant "2025-02-10T13:46:53+03:00".
    /// 
    /// This converter detects if the timezone offset is missing the colon and inserts it before attempting to parse.
    /// It ensures that the <see cref="DateTimeOffset.TryParse"/> method can correctly parse the date and time.
    /// 
    /// Usage example in a model:
    /// <code>
    /// [JsonPropertyName("published_at")]
    /// [JsonConverter(typeof(CustomDateTimeOffsetConverter))]
    /// public DateTimeOffset PublishedAt { get; set; }
    ///
    /// [JsonPropertyName("created_at")]
    /// [JsonConverter(typeof(CustomDateTimeOffsetConverter))]
    /// public DateTimeOffset CreatedAt { get; set; }
    /// </code>
    /// </remarks>
    public class CustomDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        /// <summary>
        /// Reads the JSON string and converts it to a <see cref="DateTimeOffset"/>.
        /// Adjusts non-standard timezone offsets (e.g., "+0300" to "+03:00") before parsing.
        /// </summary>
        /// <param name="reader">The JSON reader.</param>
        /// <param name="typeToConvert">The type to convert, which is <see cref="DateTimeOffset"/>.</param>
        /// <param name="options">Serialization options for JSON.</param>
        /// <returns>A valid <see cref="DateTimeOffset"/> parsed from the JSON string.</returns>
        /// <exception cref="JsonException">Thrown if the string cannot be parsed to a <see cref="DateTimeOffset"/>.</exception>
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? str = reader.GetString();

            if (!string.IsNullOrEmpty(str) && str.Length >= DateTimeOffsetConverterConstants.TimeZoneOffsetLength)
            {
                // Extract the expected timezone offset part from the end of the string.
                string lastFive = str.Substring(str.Length - DateTimeOffsetConverterConstants.TimeZoneOffsetLength);
                // Check if the offset is in a non-standard format (e.g., "+0300" or "-0300") without a colon.
                if ((lastFive[0] == '+' || lastFive[0] == '-') &&
                    char.IsDigit(lastFive[1]) && char.IsDigit(lastFive[2]) &&
                    char.IsDigit(lastFive[3]) && char.IsDigit(lastFive[4]))
                {
                    // Insert a colon at the correct position to convert it to the standard format (e.g., "+03:00" or "-03:00").
                    string correctedOffset = lastFive.Substring(0, DateTimeOffsetConverterConstants.TimeZoneHourPartLength)
                                             + ":"
                                             + lastFive.Substring(DateTimeOffsetConverterConstants.TimeZoneHourPartLength);
                    // Replace the old offset with the corrected one in the original string.
                    str = str.Substring(0, str.Length - DateTimeOffsetConverterConstants.TimeZoneOffsetLength) + correctedOffset;
                }
            }

            // Attempt to parse the corrected string into a DateTimeOffset.
            if (DateTimeOffset.TryParse(str, null, DateTimeStyles.RoundtripKind, out DateTimeOffset dto))
            {
                return dto;
            }
            throw new JsonException($"Unable to convert '{str}' to DateTimeOffset.");
        }

        /// <summary>
        /// Writes the <see cref="DateTimeOffset"/> value as a JSON string in ISO 8601 format with a timezone offset.
        /// </summary>
        /// <param name="writer">The JSON writer.</param>
        /// <param name="value">The <see cref="DateTimeOffset"/> value to write.</param>
        /// <param name="options">Serialization options for JSON.</param>
        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            // Format the date using the ISO 8601 standard with a timezone offset,
            // for example: "2025-02-16T12:26:32+03:00".
            writer.WriteStringValue(value.ToString("yyyy-MM-dd'T'HH:mm:sszzz"));
        }
    }
}
