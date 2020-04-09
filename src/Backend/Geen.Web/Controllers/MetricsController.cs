using System.Linq;
using System.Threading.Tasks;
using Geen.Web.Application.Monitoring;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Geen.Web.Controllers
{
    public class MetricsController : Controller
    {
        [HttpGet]
        [Route("/metrics")]
        public async Task Metrics()
        {
            await Response.WriteAsync("# TYPE api_rps gauge\n");

            var metrics = MetricsStorage.Get;

            await Task.WhenAll(metrics.Select(
                metric => Response.WriteAsync($"{metric.Key} {metric.Value}\n")));
        }
    }
}
