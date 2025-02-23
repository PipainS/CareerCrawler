using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHParser.Application.Interfaces.Progress
{
    /// <summary>
    /// Интерфейс для управления отдельной задачей в прогресс-баре.
    /// </summary>
    public interface IProgressTask
    {
        /// <summary>
        /// Увеличивает текущее значение задачи.
        /// </summary>
        /// <param name="amount">На сколько увеличить значение.</param>
        void Increment(double amount);
    }
}
