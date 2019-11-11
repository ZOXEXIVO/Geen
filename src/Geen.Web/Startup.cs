using System;
using System.Collections.Generic;
using Geen.Web.Application;
using Geen.Web.Application.Formatter;
using Geen.Web.Application.Prerender;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace Geen.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddHostedService<GcEventsCollector>();
            
            services.RegisterInternalServices(Configuration);

//            services.Configure<GzipCompressionProviderOptions>(
//                options => options.Level = CompressionLevel.Optimal);
//
//            services.AddResponseCompression(options =>
//            {
//                options.Providers.Add<GzipCompressionProvider>();
//                options.MimeTypes = new[]
//                {
//                    // Default
//                    "text/plain",
//                    "text/css",
//                    "application/javascript",
//                    "text/html",
//                    "application/xml",
//                    "text/xml",
//                    "application/json",
//                    "text/json",
//                    // Custom
//                    "image/svg+xml",
//                    "font/woff2",
//                    "application/x-font-ttf"
//                };
//            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });


            services.AddControllers(options =>
            {
                options.InputFormatters.Clear();
                options.InputFormatters.Add(new JsonFormatter());

                options.OutputFormatters.Clear();
                options.OutputFormatters.Add(new JsonFormatter());
            });

#if DEBUG
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "GeenApi", Version = "v1"
                });
            });
#endif
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseCors("AllowAnyOrigin");

            //app.UseResponseCompression();
            
            app.UsePrerenderedHtmls();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers.Add("cache-control", new[] {"public,max-age=31536000"});
                    context.Context.Response.Headers.Add("Expires", new[] {DateTime.UtcNow.AddYears(1).ToString("R")});
                }
            });

            app.UseSitemap();

#if DEBUG
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
#endif

            app.UseSpaFallback();

            app.UseDefaultFiles(new DefaultFilesOptions
            {
                DefaultFileNames = new List<string> {"index.html"}
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
                    context.Context.Response.Headers.Add("Expires", "-1");
                }
            });

            app.UseRpsMetrics();

            app.UserAnonymous();

            var requestLogger = loggerFactory.CreateLogger("RequestLogger");

            app.Use(async (context, func) =>
            {
                var displayUrl = context.Request.GetEncodedPathAndQuery();

                if (displayUrl != "/metrics")
                {
                    requestLogger.LogInformation("Request: {RequestUrl}", displayUrl);
                }

                await func();
            });
            
            app.UseRouting();
            app.UseEndpoints(routes =>
            {
                routes.MapAreaControllerRoute("areaRoute", "areaRoute", "api/{area:exists}/{controller}/{action}/{id?}");
                routes.MapControllerRoute("default","{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}