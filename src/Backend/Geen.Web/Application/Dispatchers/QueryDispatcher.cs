using System;
using System.Collections.Concurrent;
using Geen.Core.Interfaces.Common;
using Geen.Web.Application.Services.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Geen.Web.Application.Dispatchers
{
    public sealed class QueryDispatcher : IQueryDispatcher
    {
        private readonly ILogger<QueryDispatcher> _logger;

        private readonly IServiceProvider _serviceProvider;

        private static readonly ConcurrentDictionary<Type, Type> TypesCache = new();

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetService<ILogger<QueryDispatcher>>();
            
            _serviceProvider = serviceProvider;
        }

        public TResult Execute<TResult>(IQuery<TResult> query)
        {
            var queryType = query.GetType();
            
            var queryHandlerType = TypesCache.GetOrAdd(
                queryType,
                type => typeof(IQueryHandler<,>).MakeGenericType(type, typeof(TResult))
            );

            dynamic queryHandler = _serviceProvider.GetService(queryHandlerType);

            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug("Query: {Type}, {Body}", queryType.Name, query.ToJson());

            return queryHandler.Execute((dynamic)query);
        }
    }
}
