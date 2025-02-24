using System.ComponentModel.DataAnnotations;
using HHParser.Application.Interfaces;
using HHParser.Domain.Attributes;
using HHParser.Domain.Enums;
using HHParser.Domain.Models;
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
        /// 
        public void ShowMainMenu()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold underline]Main Menu[/]\n");

            var options = Enum.GetValues(typeof(MainMenuOption)).Cast<MainMenuOption>();

            var groupedOptions = options.GroupBy(option =>
            {
                var field = option.GetType().GetField(option.ToString());
                var groupAttr = field?.GetCustomAttributes(typeof(MenuGroupAttribute), false)
                                     .FirstOrDefault() as MenuGroupAttribute;
                return groupAttr?.GroupName ?? "Other";
            });

            var groupPanels = new List<Panel>();

            foreach (var group in groupedOptions)
            {
                var table = new Table().RoundedBorder();
                table.AddColumn(new TableColumn("Option").LeftAligned());
                table.AddColumn(new TableColumn("Description").LeftAligned());

                foreach (var option in group)
                {
                    var field = option.GetType().GetField(option.ToString());

                    var displayAttr = field?.GetCustomAttributes(typeof(DisplayAttribute), false)
                        .FirstOrDefault() as DisplayAttribute;

                    string displayName = displayAttr?.Name ?? option.ToString();

                    var statusAttr = field?.GetCustomAttributes(typeof(FeatureStatusAttribute), false)
                                            .FirstOrDefault() as FeatureStatusAttribute;

                    bool isImplemented = statusAttr?.IsImplemented ?? false;

                    if (isImplemented)
                    {
                        table.AddRow(
                            $"[green]{(int)option}[/]",
                            $"[green]V {displayName}[/]"
                        );
                    }
                    else
                    {
                        table.AddRow(
                            $"[red]{(int)option}[/]",
                            $"[red]X {displayName} (Not Implemented)[/]"
                        );
                    }
                }
                var panel = new Panel(table)
                {
                    Header = new PanelHeader($"[bold cyan]{group.Key}[/]", Justify.Center),
                    Border = BoxBorder.Rounded
                };

                groupPanels.Add(panel);
            }

            var columns = new Columns(groupPanels)
            {
                Expand = true,
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
}
