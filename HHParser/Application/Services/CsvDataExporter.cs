using System.Globalization;
using System.Text;
using CsvHelper;
using HHParser.Application.Interfaces;
using HHParser.Application.Services.Csv.Mappings;
using HHParser.Domain.Models.Vacancies;

namespace HHParser.Application.Services
{
    /// <summary>
    /// Provides functionality to export enriched vacancy data to a CSV file.
    /// This class implements the <see cref="IDataExporter"/> interface.
    /// </summary>
    public class CsvDataExporter : IDataExporter
    {
        /// <summary>
        /// Exports a collection of enriched vacancies to a CSV file.
        /// </summary>
        /// <param name="vacancies">The collection of vacancies to export.</param>
        /// <param name="baseFileName">The base file name to use for the exported CSV file.</param>
        /// <remarks>
        /// The CSV file is saved in a "Reports" folder located in the application's base directory.
        /// A timestamp is appended to the file name to ensure uniqueness.
        /// The file is written using UTF-8 encoding, and a custom class map is registered
        /// to properly map the properties of <see cref="EnrichedVacancy"/> to the CSV columns.
        /// Example usage:
        /// <code>
        /// IDataExporter exporter = new CsvDataExporter();
        /// exporter.ExportVacancies(vacancies, "VacanciesReport");
        /// </code>
        /// </remarks>
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

                Console.WriteLine($"Data successfully exported to file: {fullPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting CSV: {ex.Message}");
            }
        }
    }
}
