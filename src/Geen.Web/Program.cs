using System;
using System.IO;
using Geen.Web.Application;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

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
            ConfigureSerilog();

            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.AddSerilog();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                        {
                         
                        })
                        .UseUrls("http://*:7000")
                        .UseSockets()
                        .UseStartup<Startup>();
                }).Build();
        }

        private static void ConfigureSerilog()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .Enrich.FromLogContext()
#if DEBUG
                .WriteTo.Seq("http://localhost:5341")
                .WriteTo.Console(LogEventLevel.Information)
#else
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://172.17.0.1:9200"))
                {
                     IndexFormat = "geen-{0:yyyy.MM}",
                     AutoRegisterTemplate = true,
                })
#endif
                .CreateLogger();
        }
    }
}