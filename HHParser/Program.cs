using HHParser.Application.Interfaces;
using HHParser.Application.Services.MenuService;
using HHParser.Infrastructure.Configuration;
using HHParser.Infrastructure.Services.Api;
using HHParser.Presentation.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var services = new ServiceCollection();

// Регистрируем настройки HH API через Options pattern
services.Configure<HHApiSettings>(configuration.GetSection("HHApiSettings"));

// Регистрируем Presentation-слой
services.AddSingleton<ConsoleView>();

// Регистрируем Application-слой
services.AddSingleton<IMenuService, ConsoleMenuService>();

// Регистрируем Infrastructure-слой (работа с API)
services.AddHttpClient<IHHService, HHApiClient>();

// Если потребуется доступ к IConfiguration в других местах, можно зарегистрировать его:
services.AddSingleton<IConfiguration>(configuration);

var serviceProvider = services.BuildServiceProvider();

try
{
    var menuService = serviceProvider.GetRequiredService<IMenuService>();
    await menuService.ShowMainMenuAsync();
}
catch (Exception ex)
{
    var view = serviceProvider.GetRequiredService<ConsoleView>();
    view.ShowError($"Критическая ошибка: {ex.Message}");
}
