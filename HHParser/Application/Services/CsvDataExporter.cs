using System.Globalization;
using System.Text;
using CsvHelper;
using HHParser.Application.Interfaces;
using HHParser.Application.Services.Csv.Mappings;
using HHParser.Domain.Models.Vacancies;

namespace HHParser.Application.Services
{
    public class CsvDataExporter : IDataExporter
    {
        public void ExportVacancies(IEnumerable<EnrichedVacancy> vacancies, string baseFileName)
        {
            try
            {
                string reportsFolder = Path.Combine(AppContext.BaseDirectory, "Reports");
                Directory.CreateDirectory(reportsFolder);

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmssfff", CultureInfo.InvariantCulture);
                string fileName = $"{baseFileName}_{timestamp}.csv";
                string fullPath = Path.Combine(reportsFolder, fileName);

                using (var writer = new StreamWriter(fullPath, false, Encoding.UTF8))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<EnrichedVacancyMap>();
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
