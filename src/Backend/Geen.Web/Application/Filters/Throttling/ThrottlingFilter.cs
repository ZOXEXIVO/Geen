using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Geen.Web.Application.Filters.Throttling;

public class ThrottleFilter : Attribute, IAsyncActionFilter
{
    private static ConcurrentDictionary<string, DateTime> _clientActionActivity;
    private readonly TimeSpan _minTimeSpan;

    public ThrottleFilter(int hours, int minutes, int seconds, int milliseconds)
    {
        _minTimeSpan = new TimeSpan(0, hours, minutes, seconds, milliseconds);
        _clientActionActivity = new ConcurrentDictionary<string, DateTime>();
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.Request.Method == "OPTIONS")
            return;

        var clientIp = GetIP(context);

        if (string.IsNullOrWhiteSpace(clientIp))
            return;

        var action = $"{clientIp}_{context.RouteData.Values["Controller"]}/{context.RouteData.Values["Action"]}";

        var currentDate = DateTime.UtcNow;

        var lastActivityDate = _clientActionActivity.GetOrAdd(action, initial => currentDate);

        if (currentDate != lastActivityDate)
        {
            _clientActionActivity.AddOrUpdate(action, str => currentDate, (str, dt) => currentDate);

            if (currentDate - lastActivityDate <= _minTimeSpan)
            {
                context.Result = new StatusCodeResult(429);
                return;
            }
        }

        await next();
    }

    private string GetIP(ActionExecutingContext context)
    {
        var connectionFeature = context.HttpContext.Features.Get<IHttpConnectionFeature>();
        return connectionFeature?.RemoteIpAddress.ToString();
    }
}