using System;
using System.Collections.Concurrent;
using Geen.Core.Interfaces.Common;
using Geen.Web.Application.Services.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Geen.Web.Application.Dispatchers
{
    public sealed class CommandDispatcher : ICommandDispatcher
    {
        private readonly ILogger<CommandDispatcher> _logger;
        private readonly IServiceProvider _serviceProvider;

        private static readonly ConcurrentDictionary<Type, Type> TypesCache
            = new ConcurrentDictionary<Type, Type>();

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetService<ILogger<CommandDispatcher>>();
            
            _serviceProvider = serviceProvider;
        }

        public TResult Execute<TResult>(ICommand<TResult> command)
        {
            var commandType = command.GetType();
            
            var commandHandlerType = TypesCache.GetOrAdd(
                commandType,
                type => typeof(ICommandDispatcher<,>).MakeGenericType(type, typeof(TResult))
            );

            dynamic commandHandler = _serviceProvider.GetService(commandHandlerType);

            if(_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("Command: {Type}, {Body}", commandType.Name, command.ToJson());

            return commandHandler.Execute((dynamic)command);
        }
    }
}
