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

        [HttpGet]
        [Route("/api/player/{urlName}")]
        public Task<PlayerModel> Get(string urlName)
        {
            return _queryDispatcher.Execute(new PlayerGetByUrlName { UrlName = urlName });
        }

        [HttpGet]
        [Route("/api/players/club/{urlName}")]
        public Task<List<PlayerModel>> GetFromClub(string urlName)
        {
            return _queryDispatcher.Execute(new PlayerGetByClubUrlNameQuery
            {
                ClubUrlName = urlName
            });
        }

        [HttpGet]
        [Route("/api/players/club/{clubUrlName}/top")]
        public Task<List<PlayerModel>> GetTopPlayers(string clubUrlName)
        {
            return _queryDispatcher.Execute(new GetTopPlayerQuery
            {
                ClubUrlName = clubUrlName
            });
        }

        [HttpGet]
        [Route("/api/players/search/{query}")]
        public Task<List<PlayerModel>> SearchPlayers(string query)
        {
            return _queryDispatcher.Execute(new SearchPlayerQuery
            {
                Query = query
            });
        }
        
        [HttpGet]
        [Route("/api/players/top")]
        public Task<List<PlayerModel>> GetTopPlayers()
        {
            return _queryDispatcher.Execute(new GetTopPlayerQuery());
        }

        [HttpGet]
        [Route("/api/player/{urlName}/related")]
        public Task<List<PlayerModel>> GetRelatedPlayers(string urlName)
        {
            return _queryDispatcher.Execute(new GetRelatedPlayerQuery
            {
                UrlName = urlName
            });
        }

        [HttpGet]
        [Route("/api/player/random")]
        public Task<PlayerModel> GetRandomPlayer()
        {
            return _queryDispatcher.Execute(new GetRandomPlayerQuery());
        }
    }
}
