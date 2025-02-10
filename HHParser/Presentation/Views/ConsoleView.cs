using HHParser.Application.Interfaces;
using HHParser.Common.Extensions;
using HHParser.Domain.Enums;
using HHParser.Domain.Models;

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
            Console.Clear();
            Console.WriteLine("Main Menu:");

            foreach (MainMenuOption option in Enum.GetValues(typeof(MainMenuOption)))
            {
                Console.WriteLine($"{(int)option}. {option.GetDisplayName()}");
            }

            Console.Write("Select a menu option: ");
        }

        /// <summary>
        /// Displays a list of specialization groups along with their specialization IDs.
        /// </summary>
        /// <param name="groups">A list of specialization groups to display.</param>
        public void ShowSpecializations(List<SpecializationGroup> groups)
        {
            Console.WriteLine("\nList of groups and specializations with their IDs:\n");
            foreach (var group in groups)
            {
                Console.WriteLine($"Group (ID): {group.Id} Name: {group.Name}");
                foreach (var spec in group.Specializations ?? [])
                {
                    Console.WriteLine($"   Specialization: {spec.Name} (ID: {spec.Id})");
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Displays a list of professional roles along with their category IDs.
        /// </summary>
        /// <param name="categories">A list of professional roles categories to display.</param>
        public void ShowProfessionalRoles(List<ProfessionalRolesCategory> categories)
        {
            Console.WriteLine("\nList of professional roles:\n");
            foreach (var category in categories)
            {
                Console.WriteLine($"Category (ID): {category.Id} Name: {category.Name}");
                foreach (var role in category.Roles ?? [])
                {
                    Console.WriteLine($"   Role: {role.Name} (ID: {role.Id})");
                }
            }

            Console.WriteLine("\nPress any key to continue...");
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
