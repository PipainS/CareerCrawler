using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHParser.Application.Interfaces.Progress
{
    /// <summary>
    /// Интерфейс для работы с контекстом прогресс-бара, позволяющий создавать задачи.
    /// </summary>
    public interface IProgressContext
    {
        /// <summary>
        /// Добавляет новую задачу в прогресс-бар.
        /// </summary>
        /// <param name="description">Описание задачи.</param>
        /// <param name="maxValue">Максимальное значение (количество шагов).</param>
        /// <returns>Объект, позволяющий управлять этой задачей.</returns>
        IProgressTask AddTask(string description, double maxValue);
    }
}
