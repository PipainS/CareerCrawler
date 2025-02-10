using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HHParser.Common.Extensions
{
    /// <summary>
    /// Provides extension methods for working with enums, specifically retrieving the display name of enum values.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Retrieves the display name of an enum value by looking for the <see cref="DisplayAttribute"/> 
        /// applied to the enum field. If no attribute is found, it returns the enum value's name as a string.
        /// </summary>
        /// <param name="enumValue">The enum value to get the display name for.</param>
        /// <returns>The display name of the enum value, or the enum value's string representation if no display name is found.</returns>
        public static string GetDisplayName(this Enum enumValue)
        {
            var memberInfo = enumValue.GetType().GetMember(enumValue.ToString());

            if (memberInfo.Length > 0)
            {
                var displayAttribute = memberInfo[0].GetCustomAttribute<DisplayAttribute>();

                if (displayAttribute != null)
                {
                    return displayAttribute.GetName() ?? enumValue.ToString();
                }
            }

            return enumValue.ToString();
        }
    }
}
