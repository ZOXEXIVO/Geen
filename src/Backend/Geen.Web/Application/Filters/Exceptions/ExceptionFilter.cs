using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Geen.Web.Application.Filters.Exceptions
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            _logger.LogError("Error: " + context.Exception);
            
            context.HttpContext.Response.StatusCode = 500;

            return Task.CompletedTask;
        }
    }
}
