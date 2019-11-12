using System;
using System.Threading;
using System.Threading.Tasks;
using Geen.Web.Application.EventListener.Metrics;
using Geen.Web.Application.Monitoring;
using Microsoft.Extensions.Hosting;

namespace Geen.Web.Application.EventListener
{
    public class GcEventsCollector : IHostedService
    {
        private Timer _timer;

        private static void TickTimer(object state)
        {
            MetricsStorage.Set<Gen0Metric>( GC.CollectionCount(0));
            MetricsStorage.Set<Gen1Metric>(GC.CollectionCount(1));
            MetricsStorage.Set<Gen2Metric>(GC.CollectionCount(2));
            MetricsStorage.Set<GenHMetric>(GC.CollectionCount(3));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(TickTimer, null, 0, 3000);
            
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _timer.DisposeAsync();
        }
    }
}