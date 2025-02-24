using HHParser.Domain.Models.Vacancies;

namespace HHParser.Application.Interfaces
{
    /// <summary>
    /// Defines a method for exporting vacancy data to a file.
    /// </summary>
    public interface IDataExporter
    {
        /// <summary>
        /// Exports a collection of enriched vacancies to a file.
        /// </summary>
        /// <param name="vacancies">The collection of vacancies to export.</param>
        /// <param name="baseFileName">The base file name to use for the exported file.</param>
        void ExportVacancies(IEnumerable<EnrichedVacancy> vacancies, string baseFileName);
    }
}
