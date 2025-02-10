using HHParser.Common.Extensions;
using HHParser.Domain.Enums;
using HHParser.Domain.Models;

namespace HHParser.Presentation.Views
{
    public class ConsoleView : IConsoleView
    {
        public void ShowMainMenu()
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

        public void ShowSpecializations(List<SpecializationGroup> groups)
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

        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Ошибка: {message}");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
