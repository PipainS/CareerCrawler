using CsvHelper;
using HHParser.Application.Interfaces;
using HHParser.Application.Services.Csv.Mappings;
using HHParser.Domain.Models.Vacancies;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Text;

namespace HHParser.Infrastructure.Services.Csv
{
    /// <summary>
    /// Сервис для экспорта вакансий в CSV файл.
    /// Относится к инфраструктурному слою, так как работает с файловой системой.
    /// </summary>
    public static class CsvFileService
    {
        public static void SaveVacanciesToCsv(IEnumerable<EnrichedVacancy> vacancies, string filePath)
        {
            try
            {
                using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    // Регистрируем нашу карту
                    csv.Context.RegisterClassMap<EnrichedVacancyMap>();

                    // Пишем данные
                    csv.WriteRecords(vacancies);
                }

                Console.WriteLine($"Данные успешно сохранены в файл {filePath}");
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                Console.WriteLine($"Ошибка при сохранении в CSV: {ex.Message}");
            }
        }
    }
}
