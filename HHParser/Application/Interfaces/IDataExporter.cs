using HHParser.Domain.Models.Vacancies;

namespace HHParser.Application.Interfaces
{
    public interface IDataExporter
    {
        void ExportVacancies(IEnumerable<EnrichedVacancy> vacancies, string baseFileName);
    }

}
