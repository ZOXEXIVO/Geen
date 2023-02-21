using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Geen.Web.Controllers;

public class ErrorController : Controller
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    [HttpPost("/api/error")]
    public async Task LogBrowserError()
    {
        var requestBody = new StreamReader(Request.Body);

        _logger.LogError("BROWSER, Agent={Agent}, ErrorMessage: {ErrorMessage}",
            Request.Headers["User-Agent"].ToString(),
            await requestBody.ReadToEndAsync());
    }
}