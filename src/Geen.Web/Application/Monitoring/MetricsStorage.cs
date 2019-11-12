using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Geen.Web.Application.Monitoring
{
    public static class MetricsStorage
    {
        private const string Prefix = "geen";

        private static readonly ConcurrentDictionary<string, long> Storage = new ConcurrentDictionary<string, long>();

        public static void Inc<TMetric>()
        {
            Storage.AddOrUpdate(GetMetricName(typeof(TMetric)), 
                metric => 1, (metric, value) => value + 1);
        }
        
        public static void Set<TMetric>(long val)
        {
            Storage.AddOrUpdate(GetMetricName(typeof(TMetric)), 
                metric => 1, (metric, value) => val);
        }

        public static IDictionary<string, long> Get => Storage;

        #region Internal

        private static readonly ConcurrentDictionary<Type, string> MetricsNameCache = new ConcurrentDictionary<Type, string>();

        private static string GetMetricName(Type metricType)
        {
            return MetricsNameCache.GetOrAdd(metricType, metric => ConvertName(metricType.Name));
        }

        private static string ConvertName(string metricName)
        {
            return Prefix + "_" + string.Concat(metricName.Select(
                       (x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }

        #endregion
    }
}
