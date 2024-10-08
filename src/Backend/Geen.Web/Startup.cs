﻿using System;
using System.IO.Compression;
using Geen.Web.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Geen.Web;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var settings = services.RegisterInternalServices(Configuration);

        services.Configure<BrotliCompressionProviderOptions>(
            options => options.Level = CompressionLevel.Fastest);

        services.AddResponseCompression(options =>
        {
            options.Providers.Add<BrotliCompressionProvider>();
            options.MimeTypes = new[]
            {
                // Default
                "text/plain",
                "text/css",
                "application/javascript",
                "text/html",
                "application/xml",
                "text/xml",
                "application/json",
                "text/json",
                // Custom
                "image/svg+xml",
                "font/woff2",
                "application/x-font-ttf"
            };
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAnyOrigin",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        services.AddControllers();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "GeenApi", Version = "v1"
            });
        });
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.UseCors("AllowAnyOrigin");

        app.UseResponseCompression();

        app.UseFallbackRedirects();

        app.UseStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = context =>
            {
                context.Context.Response.Headers.Append("cache-control", new[] { "public,max-age=31536000" });
                context.Context.Response.Headers.Append("Expires", new[] { DateTime.UtcNow.AddYears(1).ToString("R") });
            }
        });

        app.UseSitemap();

#if DEBUG
        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
#endif

        app.UseStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = context =>
            {
                context.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store");
                context.Context.Response.Headers.Append("Expires", "-1");
            }
        });

        app.UserAnonymous();

        app.UseRouting();
        app.UseEndpoints(routes =>
        {
            routes.MapAreaControllerRoute("areaRoute", "Admin", "api/{area:exists}/{controller}/{action}/{id?}");
            routes.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

            routes.MapFallbackToFile("index.html");
        });
    }
}