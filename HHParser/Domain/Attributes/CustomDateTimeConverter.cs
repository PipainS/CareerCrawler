using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HHParser.Domain.Attributes
{
    public class CustomDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? str = reader.GetString();
            if (!string.IsNullOrEmpty(str) && str.Length >= 5)
            {
                // Проверяем, что последние 5 символов представляют смещение без двоеточия (например, "+0300" или "-0500")
                string lastFive = str.Substring(str.Length - 5);
                if ((lastFive[0] == '+' || lastFive[0] == '-') &&
                    char.IsDigit(lastFive[1]) && char.IsDigit(lastFive[2]) &&
                    char.IsDigit(lastFive[3]) && char.IsDigit(lastFive[4]))
                {
                    // Вставляем двоеточие для получения формата "+03:00"
                    string correctedOffset = lastFive.Substring(0, 3) + ":" + lastFive.Substring(3);
                    str = str.Substring(0, str.Length - 5) + correctedOffset;
                }
            }

            if (DateTimeOffset.TryParse(str, null, DateTimeStyles.RoundtripKind, out DateTimeOffset dto))
            {
                return dto;
            }
            throw new JsonException($"Unable to convert '{str}' to DateTimeOffset.");
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            // Записываем дату в формате ISO 8601 со смещением (например, 2025-02-16T12:26:32+03:00)
            writer.WriteStringValue(value.ToString("yyyy-MM-dd'T'HH:mm:sszzz"));
        }
    }
}
