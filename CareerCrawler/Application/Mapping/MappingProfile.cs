using AutoMapper;
using HHParser.Domain.Models.Vacancies;

namespace HHParser.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<VacancySummary, EnrichedVacancy>();
        }
    }
}
