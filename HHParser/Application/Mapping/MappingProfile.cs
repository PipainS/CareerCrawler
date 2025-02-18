using AutoMapper;
using HHParser.Domain.Models.Vacancies;

namespace HHParser.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Маппинг из VacancySummary в EnrichedVacancy
            CreateMap<VacancySummary, EnrichedVacancy>();

            // Если потребуется объединить данные из VacancyDetail
            CreateMap<VacancyDetail, EnrichedVacancy>()
                .ForMember(dest => dest.KeySkills,
                           opt => opt.MapFrom(src => src.KeySkills.Select(ks => ks.Name).ToList()));
        }
    }
}
