using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Geen.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        private static IHost BuildWebHost(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(builder =>
                {
                    builder.ClearProviders()
                        .AddFilter("Microsoft", LogLevel.Error)
                        .AddFilter("System", LogLevel.Error)
                        .AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options => { })
                        .UseUrls("http://*:7000")
                        .UseStartup<Startup>();
                }).Build();
        }
    }
}