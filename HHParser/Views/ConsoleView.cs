using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHParser.Models;

namespace HHParser.Views
{
    public class ConsoleView
    {
        public void ShowMainMenu()
        {
            Console.Clear();

            Console.WriteLine("Главное меню:");
            Console.WriteLine("1. Просмотр специализаций");
            Console.WriteLine("2. Поиск вакансий");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите пункт меню: ");
        }

        public void ShowSpecializations(List<SpecializationGroup> groups)
        {
            Console.WriteLine("\nСписок групп и специализаций с их ID:\n");
            foreach (var group in groups)
            {
                Console.WriteLine($"Группа (ID): {group.Id} Название: {group.Name}");
                foreach (var spec in group.Specializations ?? [])
                {
                    Console.WriteLine($"   Специализация: {spec.Name} (ID: {spec.Id})");
                }
            }
        }
        public HashSet<string> GetUserInputIds()
        {
            Console.WriteLine("\nВведите ID групп и/или специализаций через запятую:");
            var input = Console.ReadLine() ?? string.Empty;

            return input.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(id => id.Trim())
                .ToHashSet();
        }

        public void ShowSelectionResults(List<string> groups, List<string> specs)
        {
            Console.WriteLine("\nВаш выбор:");

            if (groups.Count != 0)
            {
                Console.WriteLine("Группы:");
                groups.ForEach(g => Console.WriteLine($"- {g}"));
            }

            if (specs.Count != 0)
            {
                Console.WriteLine("Специализации:");
                specs.ForEach(s => Console.WriteLine($"- {s}"));
            }

            if (groups.Count == 0 && specs.Count == 0)
            {
                Console.WriteLine("Ничего не найдено по введенным ID.");
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
