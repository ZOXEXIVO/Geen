using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Players;
using Geen.Core.Domains.Players.Queries;
using Geen.Core.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;

namespace Geen.Web.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public PlayerController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("/api/player/{urlName}", Name = "playerGet")]
        public Task<PlayerModel> Get(string urlName)
        {
            return _queryDispatcher.Execute(new PlayerGetByUrlName { UrlName = urlName });
        }

        [HttpGet("/api/players/club/{urlName}", Name = "clubPlayersGet")]
        public Task<List<PlayerModel>> GetFromClub(string urlName)
        {
            return _queryDispatcher.Execute(new PlayerGetByClubUrlNameQuery
            {
                ClubUrlName = urlName
            });
        }

        [HttpGet("/api/players/club/{clubUrlName}/top", Name = "topClubPlayers")]
        public Task<List<PlayerModel>> GetTopPlayers(string clubUrlName)
        {
            return _queryDispatcher.Execute(new GetTopPlayerQuery
            {
                ClubUrlName = clubUrlName
            });
        }

        [HttpGet("/api/players/search/{query}")]
        public Task<List<PlayerModel>> SearchPlayers(string query)
        {
            return _queryDispatcher.Execute(new SearchPlayerQuery
            {
                Query = query
            });
        }
        
        [HttpGet("/api/players/top", Name = "topPlayers")]
        public Task<List<PlayerModel>> GetTopPlayers()
        {
            return _queryDispatcher.Execute(new GetTopPlayerQuery());
        }

        [HttpGet("/api/player/{urlName}/related")]
        public Task<List<PlayerModel>> GetRelatedPlayers(string urlName)
        {
            return _queryDispatcher.Execute(new GetRelatedPlayerQuery
            {
                UrlName = urlName
            });
        }

        [HttpGet("/api/player/random")]
        public Task<PlayerModel> GetRandomPlayer()
        {
            return _queryDispatcher.Execute(new GetRandomPlayerQuery());
        }
    }
}
