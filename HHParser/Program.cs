//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Newtonsoft.Json;

//public class Program
//{
//    public class Root
//    {
//        public string id { get; set; }
//        public string name { get; set; }
//        public List<Specialization> specializations { get; set; }
//    }

//    public class Specialization
//    {
//        public string id { get; set; }
//        public string name { get; set; }
//        public bool laboring { get; set; }
//    }

//    public static async Task Main(string[] args)
//    {
//        var apiUrl = "https://api.hh.ru/specializations";

//        using var client = new HttpClient();

//        try
//        {
//            var response = await client.GetStringAsync(apiUrl);

//            // Десериализация JSON с использованием Newtonsoft.Json
//            var specializations = JsonConvert.DeserializeObject<List<Root>>(response);

//            Console.WriteLine("Список групп и специализаций с их ID:\n");

//            foreach(var group in specializations)
//            {
//                Console.WriteLine($"Группа №(ID): {group.id} Название: {group.name}");
//                foreach(var specialization in group.specializations)
//                {
//                    Console.WriteLine($"   Специализация: {specialization.name} (ID: {specialization.id})");
//                }
//            }

//            Console.WriteLine("\nВведите ID групп и/или специализаций через запятую:");

//            var input = Console.ReadLine();
//            if (string.IsNullOrEmpty(input))
//            {
//                Console.WriteLine("Ввод пустой. Завершение программы.");
//                return;
//            }

//            var inputIds = input
//                .Split(',', StringSplitOptions.RemoveEmptyEntries)
//                .Select(id => id.Trim())
//                .ToHashSet();

//            var selectedGroups = new List<string>();
//            var selectedSpecializations = new List<string>();

//            foreach (var group in specializations)
//            {
//                if (inputIds.Contains(group.id)) selectedGroups.Add(group.name);

//                var matchingSpecializations = group.specializations
//                    .Where(spec => inputIds.Contains(spec.id))
//                    .Select(spec => spec.name);

//                selectedSpecializations.AddRange(matchingSpecializations);
//            }

//            // Вывод результатов
//            Console.WriteLine("\nВаш выбор:");

//            if (selectedGroups.Count > 0)
//            {
//                Console.WriteLine("Группы:");
//                foreach (var groupName in selectedGroups)
//                {
//                    Console.WriteLine($"- {groupName}");
//                }
//            }

//            if (selectedSpecializations.Count > 0)
//            {
//                Console.WriteLine("Специализации:");
//                foreach (var specName in selectedSpecializations)
//                {
//                    Console.WriteLine($"- {specName}");
//                }
//            }

//            if (selectedGroups.Count == 0 && selectedSpecializations.Count == 0)
//            {
//                Console.WriteLine("Ничего не найдено по введенным ID.");
//            }

//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка: {ex.Message}");
//        }

//        Console.ReadKey();
//    }
//}

using HHParser.Models;
using HHParser.Services;

namespace HHParser
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var httpClient = new HttpClient();
            var apiService = new HHApiService(httpClient);

            // Получаем данные с API
            var specializationGroups = await apiService.GetSpecializationGroupsAsync();

            if (specializationGroups == null || specializationGroups?.Count == 0)
            {
                Console.WriteLine("Не удалось загрузить данные.");
                return;
            }

            // Вывод списка групп и специализаций
            Console.WriteLine("Список групп и специализаций с их ID:\n");
            foreach (var group in specializationGroups)
            {
                Console.WriteLine($"Группа (ID): {group.Id} Название: {group.Name}");
                foreach (var specialization in group.Specializations)
                {
                    Console.WriteLine($"   Специализация: {specialization.Name} (ID: {specialization.Id})");
                }
            }

            // Обработка пользовательского ввода
            Console.WriteLine("\nВведите ID групп и/или специализаций через запятую:");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Ввод пустой. Завершение программы.");
                return;
            }

            var inputIds = input
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(id => id.Trim())
                .ToHashSet();

            var selectedGroups = new List<string>();
            var selectedSpecializations = new List<string>();

            foreach (var group in specializationGroups)
            {
                if (inputIds.Contains(group.Id))
                {
                    selectedGroups.Add(group.Name);
                }

                var matchingSpecializations = group.Specializations
                    .Where(spec => inputIds.Contains(spec.Id))
                    .Select(spec => spec.Name);
                selectedSpecializations.AddRange(matchingSpecializations);
            }

            // Вывод результатов
            Console.WriteLine("\nВаш выбор:");

            if (selectedGroups.Any())
            {
                Console.WriteLine("Группы:");
                foreach (var groupName in selectedGroups)
                {
                    Console.WriteLine($"- {groupName}");
                }
            }

            if (selectedSpecializations.Any())
            {
                Console.WriteLine("Специализации:");
                foreach (var specName in selectedSpecializations)
                {
                    Console.WriteLine($"- {specName}");
                }
            }

            if (!selectedGroups.Any() && !selectedSpecializations.Any())
            {
                Console.WriteLine("Ничего не найдено по введенным ID.");
            }

            Console.ReadKey();
        }
    }
}
