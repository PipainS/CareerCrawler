using System.ComponentModel.DataAnnotations;
using HHParser.Application.Interfaces;
using HHParser.Common.Extensions;
using HHParser.Domain.Attributes;
using HHParser.Domain.Enums;
using HHParser.Domain.Models;
using HHParser.Domain.Models.Vacancies;
using Spectre.Console;

namespace HHParser.Presentation.Views
{
    /// <summary>
    /// Represents the console view that handles displaying different types of information to the user.
    /// </summary>
    public class ConsoleView : IConsoleView
    {
        /// <summary>
        /// Displays the main menu with options.
        /// </summary>
        public void ShowMainMenu()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold underline]Main Menu[/]\n");

            var options = Enum.GetValues(typeof(MainMenuOption)).Cast<MainMenuOption>();

            var groupedOptions = options.GroupBy(option =>
            {
                var groupAttr = option.GetType()
                    .GetField(option.ToString())
                    .GetCustomAttributes(typeof(MenuGroupAttribute), false)
                    .FirstOrDefault() as MenuGroupAttribute;
                return groupAttr?.GroupName ?? "Other";
            });

            var groupPanels = new List<Panel>();

            foreach (var group in groupedOptions)
            {
                // Создаем таблицу для элементов текущей группы
                var table = new Table().RoundedBorder();
                table.AddColumn(new TableColumn("Option").LeftAligned());
                table.AddColumn(new TableColumn("Description").LeftAligned());

                foreach (var option in group)
                {
                    // Получаем отображаемое имя
                    var displayAttr = option.GetType()
                        .GetField(option.ToString())
                        .GetCustomAttributes(typeof(DisplayAttribute), false)
                        .FirstOrDefault() as DisplayAttribute;
                    string displayName = displayAttr?.Name ?? option.ToString();

                    // Получаем статус реализации
                    var statusAttr = option.GetType()
                        .GetField(option.ToString())
                        .GetCustomAttributes(typeof(FeatureStatusAttribute), false)
                        .FirstOrDefault() as FeatureStatusAttribute;
                    bool isImplemented = statusAttr?.IsImplemented ?? false;

                    if (!isImplemented)
                    {
                        displayName += " (Not Implemented)";
                    }

                    table.AddRow($"[green]{(int)option}[/]", displayName);
                }

                // Создаем панель для группы с заголовком
                var panel = new Panel(table)
                {
                    Header = new PanelHeader($"[bold cyan]{group.Key}[/]", Justify.Center),
                    Border = BoxBorder.Rounded
                };

                groupPanels.Add(panel);
            }

            // Создаем горизонтальные колонки из панелей групп
            var columns = new Columns(groupPanels)
            {
                Expand = true, // Заставит колонки растягиваться по доступной ширине
            };

            AnsiConsole.Write(columns);
            AnsiConsole.Markup("\n[bold]Select a menu option:[/] ");
        }

        /// <summary>
        /// Displays a list of specialization groups along with their specialization IDs.
        /// </summary>
        /// <param name="groups">A list of specialization groups to display.</param>
        public void ShowSpecializations(List<SpecializationGroup> groups)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold underline]List of Groups and Specializations:[/]");

            foreach (var group in groups)
            {
                var groupPanel = new Panel(new Markup($"[bold]Group ID:[/] {group.Id}\n[bold]Group Name:[/] {group.Name}"))
                {
                    Header = new PanelHeader($"Group: {group.Name}", Justify.Center),
                    Border = BoxBorder.Rounded
                };
                AnsiConsole.Write(groupPanel);

                if (group.Specializations != null && group.Specializations.Any())
                {
                    var specTable = new Table().RoundedBorder();
                    specTable.AddColumn(new TableColumn("Specialization Name").LeftAligned());
                    specTable.AddColumn(new TableColumn("Specialization ID").Centered());

                    foreach (var spec in group.Specializations)
                    {
                        specTable.AddRow($"[yellow]{spec.Name}[/]", spec.Id.ToString());
                    }

                    AnsiConsole.Write(specTable);
                }
                else
                {
                    AnsiConsole.MarkupLine("[italic]No specializations available.[/]");
                }

                AnsiConsole.Write(new Rule());
            }

            AnsiConsole.MarkupLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Displays a list of professional roles along with their category IDs.
        /// </summary>
        /// <param name="categories">A list of professional roles categories to display.</param>
        public void ShowProfessionalRoles(List<ProfessionalRolesCategory> categories)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold underline]List of Professional Roles:[/]");

            foreach (var category in categories)
            {
                var categoryPanel = new Panel(new Markup($"[bold]Category ID:[/] {category.Id}\n[bold]Category Name:[/] {category.Name}"))
                {
                    Header = new PanelHeader($"Category: {category.Name}", Justify.Center),
                    Border = BoxBorder.Rounded
                };
                AnsiConsole.Write(categoryPanel);

                if (category.Roles != null && category.Roles.Count != 0)
                {
                    // Create a table for the roles
                    var roleTable = new Table().RoundedBorder();
                    roleTable.AddColumn(new TableColumn("Role Name").LeftAligned());
                    roleTable.AddColumn(new TableColumn("Role ID").Centered());

                    foreach (var role in category.Roles)
                    {
                        roleTable.AddRow($"[green]{role.Name}[/]", role.Id.ToString());
                    }

                    AnsiConsole.Write(roleTable);
                }
                else
                {
                    AnsiConsole.MarkupLine("[italic]No roles available.[/]");
                }

                AnsiConsole.Write(new Rule());
            }

            AnsiConsole.MarkupLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Отображает список вакансий в таблице.
        /// Принимает коллекцию объектов типа VacancySummary.
        /// </summary>
        //public void ShowVacancies(List<VacancySummary> vacancies)
        //{
        //AnsiConsole.Clear();
        //AnsiConsole.MarkupLine("[bold underline]Список вакансий[/]\n");

        //if (vacancies == null || vacancies.Count == 0)
        //{
        //    AnsiConsole.MarkupLine("[italic]Вакансии не найдены.[/]");
        //    AnsiConsole.MarkupLine("\nPress any key to continue...");
        //    Console.ReadKey();
        //    return;
        //}

        //// Создаем таблицу с округленными углами
        //var table = new Table().RoundedBorder();

        //// Добавляем колонки
        //table.AddColumn(new TableColumn("[bold]ID[/]").Centered());
        //table.AddColumn(new TableColumn("[bold]Название[/]").LeftAligned());
        //table.AddColumn(new TableColumn("[bold]Работодатель[/]").LeftAligned());
        //table.AddColumn(new TableColumn("[bold]Местоположение[/]").LeftAligned());
        //table.AddColumn(new TableColumn("[bold]Зарплата[/]").Centered());
        //table.AddColumn(new TableColumn("[bold]Дата публикации[/]").Centered());
        //table.AddColumn(new TableColumn("[bold]URL[/]").LeftAligned());

        //// Заполняем таблицу данными
        //foreach (var vacancy in vacancies)
        //{
        //    // Форматирование зарплаты
        //    string salaryStr = "не указана";
        //    if (vacancy.Salary != null)
        //    {
        //        if (vacancy.Salary.From.HasValue && vacancy.Salary.To.HasValue)
        //        {
        //            salaryStr = $"от {vacancy.Salary.From.Value} до {vacancy.Salary.To.Value} {vacancy.Salary.Currency}";
        //        }
        //        else if (vacancy.Salary.From.HasValue)
        //        {
        //            salaryStr = $"от {vacancy.Salary.From.Value} {vacancy.Salary.Currency}";
        //        }
        //        else if (vacancy.Salary.To.HasValue)
        //        {
        //            salaryStr = $"до {vacancy.Salary.To.Value} {vacancy.Salary.Currency}";
        //        }
        //    }

        //    // Определяем местоположение: берем город из Address (AddressInfo.City), если указан, иначе Area?.Name
        //    //string location = (vacancy.Address != null && !string.IsNullOrEmpty(vacancy.Address.City))
        //    //                    ? vacancy.Address.City
        //    //                    : (vacancy.Area != null ? vacancy.Area.Name : "не указано");

        //    // Форматируем дату публикации
        //    string publishedDate = vacancy.PublishedAt.ToString("dd.MM.yyyy");

        //    // Выбираем URL вакансии: если AlternateUrl указан, то берем его, иначе Url
        //    //string url = !string.IsNullOrEmpty(vacancy.AlternateUrl) ? vacancy.AlternateUrl : vacancy.Url;

        //    // Если вакансия является премиум, добавляем символ звезды к названию (свойство Title и флаг IsPremium)
        //    string title = vacancy.Name;


        //    table.AddRow(
        //        $"[green]{vacancy.Id}[/]",
        //        title,
        //        $"[yellow]{vacancy.Employer?.Name ?? "не указан"}[/]",
        //        salaryStr,
        //        publishedDate

        //    );
        //}

        //AnsiConsole.Write(table);
        //AnsiConsole.MarkupLine("\nPress any key to continue...");
        //Console.ReadKey();
        //}

        public void ShowVacancies(List<VacancySummary> vacancies)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold underline]Vacancies List[/]");

            if (vacancies == null || !vacancies.Any())
            {
                AnsiConsole.MarkupLine("[italic yellow]No vacancies found[/]");
                AnsiConsole.MarkupLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            foreach (var vacancy in vacancies)
            {
                var panelContent = new Table()
                    .HideHeaders()
                    .AddColumn(new TableColumn("Field").LeftAligned().PadRight(3))
                    .AddColumn(new TableColumn("Value").LeftAligned())
                    .Border(TableBorder.None);

                // Basic information
                AddIfExists(panelContent, "Name", vacancy.Name);
                AddIfExists(panelContent, "Premium", vacancy.Premium ? "[gold1]✓ Premium[/]" : null);

                // Salary information
                if (vacancy.Salary != null)
                {
                    var salary = vacancy.Salary;
                    var salaryParts = new List<string>();
                    if (salary.From.HasValue) salaryParts.Add($"From: [green]{salary.From}[/]");
                    if (salary.To.HasValue) salaryParts.Add($"To: [green]{salary.To}[/]");
                    salaryParts.Add($"[grey]{salary.Currency}[/]");
                    salaryParts.Add(salary.Gross ? "(Gross)" : "(Net)");

                    panelContent.AddRow("[bold]Salary[/]", string.Join(" ", salaryParts));
                }

                AddIfExists(panelContent, "Employer", vacancy.Employer?.Name);
                AddIfExists(panelContent, "Location", vacancy.Area?.Name);
                AddIfExists(panelContent, "Published", $"[grey]{vacancy.PublishedAt:g}[/]");
                AddIfExists(panelContent, "Experience", vacancy.Experience?.Name);
                AddIfExists(panelContent, "Employment", vacancy.Employment?.Name);
                AddIfExists(panelContent, "Schedule", vacancy.Schedule?.Name);

                // Key skills (for EnrichedVacancy)
                if (vacancy is EnrichedVacancy enriched && enriched.KeySkills?.Count > 0)
                {
                    var skills = string.Join(" • ", enriched.KeySkills.Select(s => $"[aqua]{s}[/]"));
                    panelContent.AddRow("[bold]Skills[/]", $"\n{skills}");
                }

                var panel = new Panel(panelContent)
                {
                    Header = new PanelHeader($"[bold deepskyblue1]{vacancy.Name.EscapeMarkup()}[/]", Justify.Left),
                    Border = BoxBorder.Rounded,
                    Padding = new Padding(1, 1, 1, 1)
                };

                AnsiConsole.Write(panel);
                AnsiConsole.WriteLine();
            }

            AnsiConsole.MarkupLine("\n[grey]Press any key to continue...[/]");
            Console.ReadKey();
        }

        private void AddIfExists(Table table, string field, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                table.AddRow($"[bold]{field}[/]", value);
            }
        }

        /// <summary>
        /// Displays an error message to the user in red.
        /// </summary>
        /// <param name="message">The error message to display.</param>
        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {message}");
            Console.ResetColor();
            Console.ReadKey();
        }
    }

    public static class StringExtensions
    {
        public static string EscapeMarkup(this string input)
        {
            return input?.Replace("[", "[[").Replace("]", "]]");
        }
    }
}
