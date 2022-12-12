using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Geen.Core.Services.Interfaces;
using Geen.Data.Caches;
using Geen.Data.Services;
using Geen.Data.Settings;
using Geen.Data.Storages.Mongo;
using Geen.Data.Storages.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization.Conventions;

namespace Geen.Data
{
    public static class Registration
    {
        public static void RegisterDataServices(this IServiceCollection services, GeenSettings settings)
        {
            RegisterMongoSettings();

            services.AddSingleton(provider => new MongoContext(
                settings.Database.MongoUrl, provider.GetService<ILogger<MongoContext>>()));

            services.AddSingleton(provider => new RedisStore(settings.Database.RedisUrl));

            services.AddSingleton<IPlayerCacheRepository, PlayerCacheRepository>();
            services.AddSingleton<IClubCacheRepository, ClubCacheRepository>();

            services.AddSingleton<IdentityService>();

            var assemblyTypes = Assembly.GetExecutingAssembly().GetTypes();

            services.RegisterRepositories(assemblyTypes);
        }

        private static void RegisterRepositories(this IServiceCollection services, IReadOnlyCollection<Type> types)
        {
            var repositories = types.Where(x => !x.IsInterface && x.FullName.EndsWith("Repository"));

            foreach (var repository in repositories)
            {
                var handlerInterface = repository.GetInterfaces().FirstOrDefault();

                if (handlerInterface == null)
                    continue;

                services.AddTransient(handlerInterface, repository);
            }
        }
        
        private static void RegisterMongoSettings()
        {
            //Mongodb Ignore extra elements
            ConventionRegistry.Register("ApplicationConventions", new ConventionPack
            {
                new IgnoreExtraElementsConvention(true)
            }, t => true);
        }
    }
}
