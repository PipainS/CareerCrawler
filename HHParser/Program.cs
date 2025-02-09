using HHParser.Application.Interfaces;
using HHParser.Application.Services.MenuService;
using HHParser.Infrastructure.Configuration;
using HHParser.Infrastructure.Services.Api;
using HHParser.Application.Commands;
using HHParser.Presentation.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

//
// Загрузка конфигурации
//
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

//
// Настройка Serilog, считывая параметры из конфигурации
//
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

var services = new ServiceCollection();

//
// Регистрируем логирование: очищаем провайдеры и добавляем Serilog
//
services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog();
});

//
// Регистрируем настройки HH API через Options pattern
// TO DO: Перенести "HHApiSettings" в класс с константами
services.Configure<HHApiSettings>(configuration.GetSection("HHApiSettings"));

//
// Регистрируем Presentation-слой
//
services.AddSingleton<ConsoleView>();

//
// Регистрируем команды меню (расширяемость функционала)
//
services.AddSingleton<IMenuCommand, SpecializationsCommand>();
// При необходимости можно добавить и другие команды:
// services.AddSingleton<IMenuCommand, VacancySearchCommand>();

//
// Регистрируем Application-слой
//
services.AddSingleton<IMenuService, ConsoleMenuService>();

//
// Регистрируем Infrastructure-слой (работа с API)
//
services.AddHttpClient<IHHService, HHApiClient>();

//
// Регистрируем IConfiguration, если потребуется в других местах
//
services.AddSingleton<IConfiguration>(configuration);

var serviceProvider = services.BuildServiceProvider();

try
{
    var menuService = serviceProvider.GetRequiredService<IMenuService>();
    using var cts = new CancellationTokenSource();
    await menuService.ShowMainMenuAsync(cts.Token);
}
catch (Exception ex)
{
    // Критическая ошибка – логируем с помощью Serilog
    Log.Fatal(ex, "Критическая ошибка в приложении.");
    var view = serviceProvider.GetRequiredService<ConsoleView>();
    view.ShowError($"Критическая ошибка: {ex.Message}");
}
finally
{
    Log.CloseAndFlush();
}
