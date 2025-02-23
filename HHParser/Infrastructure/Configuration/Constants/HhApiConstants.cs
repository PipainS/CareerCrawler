using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHParser.Infrastructure.Configuration.Constants
{
    public static class HhApiConstants
    {
        // Общее количество страниц для загрузки вакансий
        public const int TotalPages = 20;
        // Максимальное количество одновременно выполняемых запросов
        public const int MaxConcurrentRequests = 3;
        // Минимальная задержка между запросами
        public static readonly TimeSpan MinRequestDelay = TimeSpan.FromMilliseconds(1500);
        // Максимальная задержка между запросами
        public static readonly TimeSpan MaxRequestDelay = TimeSpan.FromMilliseconds(2000);

        public const int ProgressIncrementPerItem = 1;

    }

}
