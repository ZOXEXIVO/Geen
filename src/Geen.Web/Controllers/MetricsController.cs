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
        
//        [HttpGet]
//        [Route("/gc")]
//        public async Task GarbageCollector()
//        {
//            await Response.WriteAsync($"Gen0: {GcEventsCollector.Generation0} MB\r\n");
//            await Response.WriteAsync($"Gen1: {GcEventsCollector.Generation1} MB\r\n");
//            await Response.WriteAsync($"Gen2: {GcEventsCollector.Generation2} MB\r\n");
//            await Response.WriteAsync($"GenL: {GcEventsCollector.GenerationLoh} MB\r\n");
//        }
    }
}
