using HHParser.Configuration;
using HHParser.Services.HHService;
using HHParser.Services.HHService.Interfaces;
using HHParser.Services.MenuService;
using HHParser.Services.MenuService.Interfaces;
using HHParser.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var services = new ServiceCollection();

services.AddSingleton<IConfiguration>(configuration);
services.AddHttpClient<IHHService, HHApiService>();
services.AddSingleton<ConsoleView>();
services.AddSingleton<IMenuService, ConsoleMenuService>();

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