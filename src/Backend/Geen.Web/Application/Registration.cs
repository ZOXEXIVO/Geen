using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geen.Core;
using Geen.Core.Interfaces.Common;
using Geen.Data;
using Geen.Data.Settings;
using Geen.Web.Application.Authentication.Services;
using Geen.Web.Application.Constants;
using Geen.Web.Application.Dispatchers;
using Geen.Web.Application.Monitoring;
using Geen.Web.Application.Monitoring.Metrics;
using Geen.Web.Application.Monitoring.Metrics.Bot;
using Geen.Web.Application.Services.Json;
using Geen.Web.Application.Sitemap;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;

namespace Geen.Web.Application
{
    public static class ServiceRegistration
    {
        public static void RegisterInternalServices(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = services.RegisterSettings(configuration);

            services.RegisterDataServices(settings);
            services.RegisterCoreServices();
            
            services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            
            services.AddSingleton<UserService>();
            services.AddTransient<SitemapProvider>();
            services.AddTransient<AuthenticationService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        private static GeenSettings RegisterSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            var settingsSection = configuration.GetSection("Settings");

            services.Configure<GeenSettings>(settingsSection);

            return settingsSection.Get<GeenSettings>();
        }

        public static void UserAnonymous(this IApplicationBuilder app)
        {
            app.Use(async (context, func) =>
            {
                if (!context.Request.Cookies.ContainsKey(CookieConstants.AuthCookieName))
                {
                    var geenId = ObjectId.GenerateNewId().ToString();

                    context.Response.Cookies.Append(CookieConstants.AuthCookieName, geenId,
                        new CookieOptions { Expires = DateTime.UtcNow.AddYears(100) });
                }

                await func();
            });
        }

        private static readonly List<PathString> ApiPaths = new List<PathString>
        {
            new PathString("/api"),
            new PathString("/metrics"),
            new PathString("/gc"),
            new PathString("/signin"),
            new PathString("/signout")
        };

        private static readonly List<PathString> GonePaths = new List<PathString>
        {
            new PathString("/mention"),
            new PathString("/page"),
            new PathString("/club/list")
        };

        public static void UseFallbackRedirects(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                if (GonePaths.Any(path => context.Request.Path.StartsWithSegments(path)))
                {
                    context.Response.StatusCode = 410; //gone
                    return;
                }

                if (ApiPaths.All(path => !context.Request.Path.StartsWithSegments(path)))
                {
                    if (context.Request.Path.ToString().EndsWith("page/1"))
                    {
                        context.Response.Redirect(context.Request.Path.ToString().Replace("/page/1", ""), true);
                        return;
                    }
                }
                
                await next();
            });
        }

        public static void UseSitemap(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                if (string.CompareOrdinal(context.Request.Path, "/sitemap.xml") == 0)
                {
                    var sitemapProvider = context.RequestServices.GetService<SitemapProvider>();

                    await context.Response.WriteAsync(
                        await sitemapProvider.Generate(), Encoding.UTF8);

                    MetricsStorage.Inc<SitemapCrawledMetric>();

                    return;
                }

                await next();
            });
        }

        public static void UseRpsMetrics(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await next();

                if (string.CompareOrdinal(context.Request.Path, "/metrics") != 0)
                {
                    if (context.Response.StatusCode == 200)
                        MetricsStorage.Inc<RequestPerSecondMetric>();
                }
            });
        }        
    }
}
