using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Leagues;
using Geen.Core.Domains.Leagues.Queries;
using Geen.Core.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;

namespace Geen.Web.Controllers
{
    public class LeagueController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public LeagueController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [Route("/api/league/list")]
        public Task<List<LeagueModel>> Get()
        {
            return _queryDispatcher.Execute(new LeagueGetListQuery());
        }
    }
}
