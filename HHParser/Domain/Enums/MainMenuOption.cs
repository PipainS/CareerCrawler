using System.ComponentModel.DataAnnotations;

namespace HHParser.Domain.Enums
{
    /// <summary>
    /// Represents main menu options for the application
    /// </summary>
    public enum MainMenuOption
    {
        /// <summary>
        /// Option to view specializations
        /// </summary>
        [Display(Name = "View Specializations")]
        Specializations = 1,

        /// <summary>
        /// Option to search vacancies
        /// </summary>
        [Display(Name = "Vacancy Search")]
        VacancySearch = 2,

        /// <summary>
        /// Option to search professional roles
        /// </summary>
        [Display(Name = "View Professional Roles")]
        ProfessionalRolesSearch = 3,

        /// <summary>
        /// Option to exit the application
        /// </summary>
        [Display(Name = "Exit")]
        Exit = 0
    }
}