using HHParser.Domain.Models.Vacancies;
using System.Collections.Generic;

namespace HHParser.Application.Interfaces
{
    /// <summary>
    /// Контракт для сервиса экспорта вакансий в CSV файл.
    /// </summary>
    public interface ICsvFileService
    {
        /// <summary>
        /// Сохраняет коллекцию обогащённых вакансий в CSV файл.
        /// </summary>
        /// <param name="vacancies">Коллекция вакансий для экспорта.</param>
        /// <param name="filePath">Путь к файлу для сохранения данных.</param>
        void SaveVacanciesToCsv(IEnumerable<EnrichedVacancy> vacancies, string filePath);
    }
}
