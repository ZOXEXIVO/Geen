using System.IO;
using Geen.Core.Interfaces.Common;
using Geen.Data.Settings;
using Geen.Web.Application.Monitoring;
using Geen.Web.Application.Monitoring.Metrics.Bot;
using Geen.Web.Application.Prerender.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Geen.Web.Application.Prerender
{
    public static class PrerenderMiddeware
    {
        private const string PrerenderUrlParam = "?_escaped_fragment_=";

        public static void UsePrerenderedHtmls(this IApplicationBuilder app)
        {
            app.Use(async (context, func) =>
            {
                var displayUrl = context.Request.GetEncodedPathAndQuery();

                if (displayUrl.StartsWith("/api"))
                {
                    await func();
                    return;
                }

                var userAgent = context.Request.Headers["User-Agent"].ToString();

                var isYandex = userAgent.IsYandexBot();
                var isGoogle = userAgent.IsGoogleBot();

                if(isYandex)
                    MetricsStorage.Inc<YandexCrawledMetric>();
                if (isGoogle)
                    MetricsStorage.Inc<GoogleCrawledMetric>();

                if (displayUrl.EndsWith(PrerenderUrlParam))
                {
                    if (isYandex)
                        MetricsStorage.Inc<YandexCrawledPrerenderedMetric>();
                    if (isGoogle)
                        MetricsStorage.Inc<GoogleCrawledPrerenderedMetric>();

                    var staticPath = context.RequestServices
                        .GetService<IOptions<GeenSettings>>()
                        .Value.Prerender.StaticPath;

                    var urlPath = context.Request.Path.ToString();

                    if (urlPath.EndsWith("/"))
                        urlPath = urlPath.Remove(urlPath.Length - 1);

                    if (urlPath.Length == 0)
                        urlPath = "/index";

                    var staticFilePath = staticPath + urlPath + ".html";

                    if (!File.Exists(staticFilePath))
                    {
                        var logger = context.RequestServices.GetService<ILogger<Program>>();

                        logger.LogError("Path: {Path} does not exist", staticFilePath);

                        context.Response.StatusCode = 404;
                        return;
                    }

                    using var stream = new FileStream(staticFilePath, FileMode.Open, FileAccess.Read);
                    
                    context.Response.ContentType = "text/html";

                    await stream.CopyToAsync(context.Response.Body);

                    return;
                }

                await func();
            });
        }

    }
}
