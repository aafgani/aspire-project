using App.Business.Extensions;
using App.Function;
using App.Function.Extensions;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

IConfigurationRoot configRoot = null;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication(builder =>
    {
        //builder.UseMiddleware<LoggingMiddleware>();  // Add middleware
    })
    .ConfigureAppConfiguration(configuration =>
    {
        //configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
        configRoot = configuration.Build();
    })
    .ConfigureServices( services =>
    {
        //services.AddLogging();
        services
            .AddCustomConfiguration()
            .AddCustomServices()
            .AddBusinesServices(configRoot);

        //Application Insights 
        //services.AddApplicationInsightsTelemetryWorkerService().
        //services.ConfigureFunctionsApplicationInsights();

    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var initializationService = scope.ServiceProvider.GetRequiredService<InitializationService>();
    await initializationService.StartAsync(default);  // Ensures queue exists
}

host.Run();
