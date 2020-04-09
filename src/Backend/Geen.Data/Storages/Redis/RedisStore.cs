using System;
using StackExchange.Redis;

namespace Geen.Data.Storages.Redis
{
    public class RedisStore
    {
        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;

        public RedisStore(string redisUrl)
        {
            _lazyConnection = new Lazy<ConnectionMultiplexer>(
                () => ConnectionMultiplexer.Connect(redisUrl));
        }

        public IDatabase Current => _lazyConnection.Value.GetDatabase();
    }
}
