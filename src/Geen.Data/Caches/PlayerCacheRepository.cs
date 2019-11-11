using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Geen.Core.Domains.Players;
using Geen.Core.Domains.Players.Queries;
using Geen.Core.Interfaces.Common;
using Geen.Core.Models.Content;
using Geen.Core.Models.Mentions;
using Geen.Core.Services.Interfaces;

namespace Geen.Data.Caches
{
    public class PlayerCacheRepository : IPlayerCacheRepository
    {
        private readonly Lazy<Task<ConcurrentBag<PlayerModel>>> _cache;

        public PlayerCacheRepository(IQueryDispatcher queryDispatcher)
        {
            _cache = new Lazy<Task<ConcurrentBag<PlayerModel>>>(
                () => queryDispatcher.Execute(new PlayerForCacheQuery()));
        }

        public EntityInfo GetPlayerUrl(string word, ContentContext context)
        {
            var players = _cache.Value.Result
                .Where(player => word.StartsWith(player.LastName))
                .ToList();

            if (players.Count == 0)
                return EntityInfo.Empty;

            if (players.Count == 1)
            {
                var player = players.SingleOrDefault();

                if(player.Id == context.Player?.Id || player.Id == 20)
                    return EntityInfo.Empty;

                var currentPlayer = players.SingleOrDefault();

                return new EntityInfo
                {
                    Id = currentPlayer.Id,
                    Url = currentPlayer.UrlName
                };
            }
                
            var clubPlayers = players
                .Where(x => x.Club?.Id == context.Club?.Id)
                .ToList();

            if (clubPlayers.Count == 1)
            {
                var clubPlayer = clubPlayers.SingleOrDefault();

                if (clubPlayer.Id == context.Player?.Id || clubPlayer.Id == 20)
                    return EntityInfo.Empty;

                return new EntityInfo
                {
                    Id = clubPlayer.Id,
                    Url = clubPlayer.UrlName
                };
            }

            return EntityInfo.Empty;
        }
    }
}
