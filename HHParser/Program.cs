using HHParser.Application.Interfaces;
using HHParser.Application.Mapping;
using HHParser.Application.Services.MenuService;
using HHParser.Infrastructure.Configuration;
using HHParser.Infrastructure.Configuration.Constants;
using HHParser.Infrastructure.Services.Api;
using HHParser.Presentation.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

#region Configuration Loading
// Loading the configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();
#endregion

#region Serilog Setup
// Setting up Serilog, reading parameters from the configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
#endregion

#region Service Registration
var services = new ServiceCollection();

// Registering logging: clearing existing providers and adding Serilog
services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog();
});

services.AddMemoryCache();

// Registering HH API settings using Options pattern
services.Configure<HHApiSettings>(configuration.GetSection(ConfigurationKeys.HHApiSettingsSection));

services.AddAutoMapper(typeof(MappingProfile));

// Registering Presentation layer
services.AddSingleton<IConsoleView, ConsoleView>();

// Automated registration of menu commands
services.Scan(scan => scan
    .FromAssemblyOf<IMenuCommand>()
    .AddClasses(classes => classes.AssignableTo<IMenuCommand>())
    .AsImplementedInterfaces()
    .WithSingletonLifetime());

// Registering Application layer
services.AddSingleton<IMenuService, ConsoleMenuService>();

// Registering Infrastructure layer (working with the API)
services.AddHttpClient<IHeadHunterApiClient, HeadHunterApiClient>();

// Registering IConfiguration for use in other places if needed
services.AddSingleton<IConfiguration>(configuration);
#endregion

#region Building Service Provider
var serviceProvider = services.BuildServiceProvider();
#endregion

#region Main Program Execution
try
{
    var menuService = serviceProvider.GetRequiredService<IMenuService>();
    using var cts = new CancellationTokenSource();
    await menuService.ShowMainMenuAsync(cts.Token);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Critical error in the application.");
    var view = serviceProvider.GetRequiredService<IConsoleView>();
    view.ShowError($"Critical error: {ex.Message}");
}
finally
{
    Log.CloseAndFlush();
}
#endregion
