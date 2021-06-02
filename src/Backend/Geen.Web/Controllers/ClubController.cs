using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Clubs;
using Geen.Core.Domains.Clubs.Queries;
using Geen.Core.Domains.Players;
using Geen.Core.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;

namespace Geen.Web.Controllers
{
    public class ClubController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ClubController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("/api/club/list")]
        public Task<List<ClubModel>> Get()
        {
            return _queryDispatcher.Execute(new ClubGetListQuery());
        }

        [HttpGet("/api/club/{urlName}")]
        public Task<ClubModel> Get(string urlName)
        {
            return _queryDispatcher.Execute(new ClubGetByUrlNameQuery
            {
                UrlName = urlName
            });
        }
        
        [HttpGet("/api/club/{urlName}/coach")]
        public Task<PlayerModel> Coach(string urlName)
        {
            return _queryDispatcher.Execute(new ClubGetCoachQuery
            {
                ClubUrlName = urlName
            });
        }

        [HttpGet("/api/club/{urlName}/age/average")]
        public Task<double?> GetAverageAge(string urlName)
        {
            return _queryDispatcher.Execute(new ClubGetAverageAgeQuery
            {
                UrlName = urlName
            });
        }
    }
}
