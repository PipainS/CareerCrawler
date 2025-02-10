using System.ComponentModel.DataAnnotations;

namespace HHParser.Domain.Enums
{
    public enum MainMenuOption
    {
        [Display(Name = "Просмотр специализаций")]
        Specializations = 1,

        [Display(Name = "Поиск вакансий")]
        VacancySearch = 2,

        [Display(Name = "Поиск профессиональных ролей")]
        ProfessionalRolesSearch = 3,

        [Display(Name = "Выход")]
        Exit = 0
    }
}
