using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Leagues;
using Geen.Core.Domains.Leagues.Commands;
using Geen.Core.Domains.Leagues.Queries;
using Geen.Core.Interfaces.Common;
using Geen.Web.Application.Authentication.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Geen.Web.Controllers.Admin
{
    [AuthenticationFilter]
    public class AdminLeagueController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public AdminLeagueController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        [HttpGet("api/admin/league", Name = "getAdminLeagueList")]
        public Task<List<LeagueModel>> List()
        {
            return _queryDispatcher.Execute(new LeagueGetListQuery());
        }

        [HttpGet("api/admin/league/{id:int}", Name = "getAdminLeague")]
        public Task<LeagueModel> Get(int id)
        {
            return _queryDispatcher.Execute(new LeagueGetByIdQuery { Id = id });
        }

        [HttpPost("api/admin/league", Name = "saveAdminLeague")]
        public Task Save([FromBody]LeagueModel obj)
        {
            return _commandDispatcher.Execute(new LeagueSaveCommand
            {
                Model = obj
            });
        }
    }
}
