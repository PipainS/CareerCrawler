using HHParser.Common.Extensions;
using HHParser.Domain.Enums;
using HHParser.Domain.Models;

namespace HHParser.Presentation.Views
{
    /// <summary>
    /// При масштабировании проекта и возможном переходете 
    /// на GUI класс не является статическим
    /// </summary>
    public class ConsoleView
    {
#pragma warning disable CA1822
#pragma warning disable S2325
        public void ShowMainMenu()
#pragma warning restore S2325
#pragma warning restore CA1822
        {
            Console.Clear();
            Console.WriteLine("Главное меню:");

            // Получаем все значения перечисления, кроме тех, которые не нужно выводить (если требуется)
            foreach (MainMenuOption option in Enum.GetValues(typeof(MainMenuOption)))
            {
                Console.WriteLine($"{(int)option}. {option.GetDisplayName()}");
            }

            Console.Write("Выберите пункт меню: ");
        }

#pragma warning disable CA1822
#pragma warning disable S2325
        public void ShowSpecializations(List<SpecializationGroup> groups)
#pragma warning restore S2325
#pragma warning restore CA1822
        {
            Console.WriteLine("\nСписок групп и специализаций с их ID:\n");
            foreach (var group in groups)
            {
                Console.WriteLine($"Группа (ID): {group.Id} Название: {group.Name}");
                foreach (var spec in group.Specializations ?? new List<Specialization>())
                {
                    Console.WriteLine($"   Специализация: {spec.Name} (ID: {spec.Id})");
                }
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        public void ShowProfessionalRoles(List<ProfessionalRolesCategory> categories)
        {
            Console.WriteLine("\nСписок профессиональных ролей:\n");
            foreach (var category in categories)
            {
                Console.WriteLine($"Категория (ID): {category.Id} Название: {category.Name}");
                foreach (var role in category.Roles ?? new List<ProfessionalRole>())
                {
                    Console.WriteLine($"   Роль: {role.Name} (ID: {role.Id})");
                }
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2325:Make methods static", 
            Justification = "Methods are kept non-static for potential future instance dependencies.")]
#pragma warning disable CA1822
        public void ShowError(string message)
#pragma warning restore CA1822
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Ошибка: {message}");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
