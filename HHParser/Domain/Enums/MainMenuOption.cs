using System.ComponentModel.DataAnnotations;

namespace HHParser.Domain.Enums
{
    public enum MainMenuOption
    {
        [Display(Name = "Выход")]
        Exit = 0,

        [Display(Name = "Просмотр специализаций")]
        Specializations = 1,

        [Display(Name = "Поиск вакансий")]
        VacancySearch = 2
    }
}
