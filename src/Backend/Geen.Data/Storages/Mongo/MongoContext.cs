using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Geen.Data.Entities.Attributes;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.Core.Events;

namespace Geen.Data.Storages.Mongo
{
    public class MongoContext : IDisposable
    {
        private readonly ConcurrentDictionary<Type, string> _collectionNameCache = new();

        private readonly MongoUrl _mongoUrl;
        private readonly Lazy<MongoClient> _mongoClient;

        public MongoContext(string mongoUrl, ILogger<MongoContext> logger)
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(mongoUrl));

            settings.ClusterConfigurator =
                cb => GetClusterBulder(cb, logger);

            settings.WaitQueueTimeout = TimeSpan.FromSeconds(30);

            _mongoUrl = new MongoUrl(mongoUrl);
            _mongoClient = new Lazy<MongoClient>(() => new MongoClient(settings));
        }

        public IMongoCollection<T> For<T>()
        {
            return GetCollection<T>();
        }

        public IMongoClient Client => _mongoClient.Value;

        #region Internal

        private string GetCollectionName(Type entityType)
        {
            var collectionAttr = (MongoEntityAttribute)entityType.GetTypeInfo().GetCustomAttribute(typeof(MongoEntityAttribute));

            if (collectionAttr == null)
                throw new InvalidOperationException($"Entity with type '{entityType.GetTypeInfo().FullName}' is not MongoDB entity");

            if (string.IsNullOrWhiteSpace(collectionAttr.SchemaName))
                return collectionAttr.CollectionName;

            return $"{collectionAttr.SchemaName}.{collectionAttr.CollectionName}";
        }

        private IMongoCollection<TEntity> GetCollection<TEntity>()
        {
            var database = _mongoClient.Value.GetDatabase(_mongoUrl.DatabaseName);

            return database.GetCollection<TEntity>(_collectionNameCache.GetOrAdd(typeof(TEntity), GetCollectionName));
        }

        #endregion

        #region Logging

        private static readonly JsonWriterSettings JsonWriterSettings = new()
        {
            Indent = true
        };

        private readonly HashSet<string> _ignoringCommands = new()
        {
            "isMaster",
            "buildInfo",
            "getLastError"
        };

        private void GetClusterBulder(ClusterBuilder builder, ILogger<MongoContext> logger)
        {
            builder.Subscribe<CommandStartedEvent>(e =>
            {
                if (_ignoringCommands.Contains(e.CommandName))
                    return;
            });

            builder.Subscribe<CommandSucceededEvent>(e =>
            {
                if (_ignoringCommands.Contains(e.CommandName))
                    return;

                if (e.Duration.TotalMilliseconds > 100)
                {
                    logger.LogWarning("LONG REQUEST: {CommandName}, {Duration} ms ", e.CommandName, e.Duration.TotalMilliseconds);
                }
            });
        }

        #endregion

        public void Dispose()
        {
        }
    }
}
