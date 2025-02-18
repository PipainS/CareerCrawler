using System.ComponentModel.DataAnnotations;
using HHParser.Domain.Attributes;

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
        //[Display(Name = "View Specializations")]
        //Specializations = 1,

        ///// <summary>
        ///// Option to search vacancies
        ///// </summary>
        //[Display(Name = "Vacancy Search")]
        //VacancySearch = 2,

        ///// <summary>
        ///// Option to search professional roles
        ///// </summary>
        //[Display(Name = "View Professional Roles")]
        //ProfessionalRolesSearch = 3,

        ///// <summary>
        ///// Option to exit the application
        ///// </summary>
        //[Display(Name = "Exit")]
        //Exit = 0

        [Display(Name = "Exit")]
        [MenuGroup("System")]
        [FeatureStatus(true)]
        Exit = 0,

        [Display(Name = "View Specializations")]
        [MenuGroup("Dictionary Data")]
        [FeatureStatus(true)]
        Specializations = 1,

        [Display(Name = "Vacancy Search")]
        [MenuGroup("Vacancy Data")]
        [FeatureStatus(false)]
        VacancySearch = 2,

        [Display(Name = "View Professional Roles")]
        [MenuGroup("Dictionary Data")]
        [FeatureStatus(true)]
        ProfessionalRolesSearch = 3,

        [Display(Name = "Employer Search")]
        [MenuGroup("Vacancy Data")]
        [FeatureStatus(false)]
        EmployerSearch = 4,

        [Display(Name = "View Areas")]
        [MenuGroup("Dictionary Data")]
        [FeatureStatus(false)]
        Areas = 5,

        [Display(Name = "Vacancy Details")]
        [MenuGroup("Vacancy Data")]
        [FeatureStatus(false)]
        VacancyDetails = 6,

        [Display(Name = "View Statistics")]
        [MenuGroup("Data Visualization")]
        [FeatureStatus(false)]
        Statistics = 7,
    }
}