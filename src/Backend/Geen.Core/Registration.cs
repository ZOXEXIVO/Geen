using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Geen.Core.Services.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Geen.Core
{
    public static class Registration
    {
        public static void RegisterCoreServices(this IServiceCollection services)
        {
            services.AddSingleton<ITextService, TextService>();
            services.AddSingleton<IContentService, ContentService>();

            var assemblyTypes = Assembly.GetExecutingAssembly().GetTypes();

            services.RegisterCommandsAndQueries(assemblyTypes);
        }

        private static void RegisterCommandsAndQueries(this IServiceCollection services, IReadOnlyCollection<Type> types)
        {
            var queries = types.Where(x => x.FullName.EndsWith("QueryHandler"));

            foreach (var handler in queries)
            {
                var handlerInterface = handler.GetInterfaces().FirstOrDefault();

                if (handlerInterface == null)
                    continue;

                services.AddTransient(handlerInterface, handler);
            }

            var commands = types.Where(x => x.FullName.EndsWith("CommandDispatcher"));
            foreach (var dispatcher in commands)
            {
                var dispatcherInterface = dispatcher.GetInterfaces().FirstOrDefault();

                if (dispatcherInterface == null)
                    continue;

                services.AddTransient(dispatcherInterface, dispatcher);
            }
        }
    }
}
