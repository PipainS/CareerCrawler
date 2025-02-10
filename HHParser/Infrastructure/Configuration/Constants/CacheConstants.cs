using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHParser.Infrastructure.Configuration.Constants
{
    public static class CacheConstants
    {
        /// <summary>
        /// Время жизни кэша (например, 24 часа)
        /// </summary>
        public static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);

        /// <summary>
        /// Ключ для кэширования специализаций
        /// </summary>
        public const string SpecializationsCacheKey = "specializations";

        /// <summary>
        /// Ключ для кэширования профессиональных ролей
        /// </summary>
        public const string ProfessionalRolesCacheKey = "professionalRoles";
    }
}
