using HHParser.Domain.Models;
using HHParser.Domain.Enums;

namespace HHParser.Presentation.Views
{
    /// <summary>
    /// Интерфейс для представления пользовательского интерфейса.
    /// </summary>
    public interface IConsoleView
    {
        /// <summary>
        /// Вывод главного меню.
        /// </summary>
        void ShowMainMenu();

        /// <summary>
        /// Вывод списка групп и специализаций.
        /// </summary>
        /// <param name="groups">Список групп специализаций.</param>
        void ShowSpecializations(List<SpecializationGroup> groups);

        /// <summary>
        /// Вывод списка профессиональных ролей.
        /// </summary>
        /// <param name="categories">Список категорий профессиональных ролей.</param>
        void ShowProfessionalRoles(List<ProfessionalRolesCategory> categories);

        /// <summary>
        /// Вывод сообщения об ошибке.
        /// </summary>
        /// <param name="message">Текст сообщения об ошибке.</param>
        void ShowError(string message);
    }
}
