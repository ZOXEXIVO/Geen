using Geen.Jobs.Application.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<SportsSyncService>();
    })
    .ConfigureLogging(builder =>
    {
        builder.ClearProviders()
            .AddFilter("Microsoft", LogLevel.Error)
            .AddFilter("System", LogLevel.Error)
            .AddConsole();
    })
    .Build()
    .RunAsync();