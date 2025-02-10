using HHParser.Domain.Models;

namespace HHParser.Application.Interfaces
{
    /// <summary>
    /// Represents a console view for interacting with the user.
    /// Provides methods for displaying menus, lists of specialization groups and professional roles,
    /// as well as error messages.
    /// </summary>
    public interface IConsoleView
    {
        /// <summary>
        /// Displays the main menu to the user.
        /// </summary>
        void ShowMainMenu();

        /// <summary>
        /// Displays a list of specialization groups.
        /// </summary>
        /// <param name="groups">A list of specialization groups to be displayed.</param>
        void ShowSpecializations(List<SpecializationGroup> groups);

        /// <summary>
        /// Displays a list of professional roles categories.
        /// </summary>
        /// <param name="categories">A list of professional roles categories to be displayed.</param>
        void ShowProfessionalRoles(List<ProfessionalRolesCategory> categories);

        /// <summary>
        /// Displays an error message to the user.
        /// </summary>
        /// <param name="message">The text of the error message.</param>
        void ShowError(string message);
    }
}
