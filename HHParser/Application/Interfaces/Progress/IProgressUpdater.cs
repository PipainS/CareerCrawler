using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHParser.Application.Interfaces.Progress
{
    public interface IProgressUpdater
    {
        /// <summary>
        /// Увеличивает текущее значение прогресса.
        /// </summary>
        /// <param name="amount">На сколько увеличить значение (обычно 1 за шаг).</param>
        void Increment(double amount);
    }

}
