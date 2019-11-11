//using System.Diagnostics.Tracing;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Hosting;
//
//namespace Geen.Web.Application.Monitoring.EventsListener
//{
//    public class GcEventsCollector : EventListener, IHostedService
//    {
//        public static ulong Generation0; 
//        public static ulong Generation1; 
//        public static ulong Generation2; 
//        public static ulong GenerationLoh; 
//        
//        public Task StartAsync(CancellationToken cancellationToken)
//        {
//            return Task.CompletedTask;
//        }
//
//        protected override void OnEventSourceCreated(EventSource eventSource)
//        {
//            if (eventSource.Name.Equals("Microsoft-Windows-DotNETRuntime"))
//            {
//                EnableEvents(eventSource, EventLevel.Informational, (EventKeywords)0x1);
//            }
//        }
//
//        protected override void OnEventWritten(EventWrittenEventArgs eventData)
//        {
//            switch (eventData.EventName)
//            {
//                case "GCHeapStats_V1":
//                    ProcessHeapStats(eventData);
//                    break;
//            }
//        }
//        
//        private void ProcessHeapStats(EventWrittenEventArgs eventData)
//        {
//            Generation0 = (ulong)eventData.Payload[0] / 1024 * 1024;
//            Generation1 = (ulong)eventData.Payload[2]  / 1024 * 1024;
//            Generation2 = (ulong)eventData.Payload[4]  / 1024 * 1024;
//            GenerationLoh = (ulong)eventData.Payload[6] / 1024 * 1024;
//        }
//        
//        public Task StopAsync(CancellationToken cancellationToken)
//        {
//            return Task.CompletedTask;
//        }
//    }
//}