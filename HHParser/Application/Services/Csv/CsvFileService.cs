using CsvHelper;
using HHParser.Application.Interfaces;
using HHParser.Application.Services.Csv.Mappings;
using HHParser.Domain.Models.Vacancies;
using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Сохраняет вакансии в CSV файл.
        /// Файл сохраняется в папку "Reports" (будет создана, если её нет), 
        /// а в названии файла добавляется метка времени для уникальности.
        /// </summary>
        /// <param name="vacancies">Коллекция вакансий для экспорта</param>
        /// <param name="baseFileName">Базовое имя файла (без расширения), по умолчанию "vacancies"</param>
        public static void SaveVacanciesToCsv(IEnumerable<EnrichedVacancy> vacancies, string baseFileName = "vacancies")
        {
            try
            {
                // Определяем путь к папке Reports, относительно текущего каталога приложения
                string reportsFolder = Path.Combine(AppContext.BaseDirectory, "Reports");

                // Создаем папку, если она не существует
                Directory.CreateDirectory(reportsFolder);

                // Формируем уникальное имя файла с датой и временем (с миллисекундами)
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmssfff", CultureInfo.InvariantCulture);
                string fileName = $"{baseFileName}_{timestamp}.csv";
                string fullPath = Path.Combine(reportsFolder, fileName);

                // Записываем данные в CSV файл
                using (var writer = new StreamWriter(fullPath, false, Encoding.UTF8))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    // Регистрируем карту маппинга для EnrichedVacancy
                    csv.Context.RegisterClassMap<EnrichedVacancyMap>();

                    // Пишем данные
                    csv.WriteRecords(vacancies);
                }

                Console.WriteLine($"Данные успешно сохранены в файл: {fullPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении в CSV: {ex.Message}");
            }
        }
    }
}
