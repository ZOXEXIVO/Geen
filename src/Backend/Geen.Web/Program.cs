using System.Linq;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
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
                .AddPrometheusMetrics()
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

    static class ProgramExtensions
    {
        public static IHostBuilder AddPrometheusMetrics(this IHostBuilder hostBuilder)
        {
            var metrics = AppMetrics.CreateDefaultBuilder()
                .OutputMetrics.AsPrometheusPlainText()
                .OutputMetrics.AsPrometheusProtobuf()
                .Build();

            hostBuilder.ConfigureMetrics(metrics)
                .UseMetricsWebTracking()
                .UseMetrics(options =>
                {
                    options.EndpointOptions = endpointsOptions =>
                    {
                        endpointsOptions.MetricsTextEndpointOutputFormatter = metrics.OutputMetricsFormatters
                            .OfType<MetricsPrometheusTextOutputFormatter>().First();
                        endpointsOptions.MetricsEndpointOutputFormatter = metrics.OutputMetricsFormatters
                            .OfType<MetricsPrometheusProtobufOutputFormatter>().First();
                    };
                });

            return hostBuilder;
        }
    }
}