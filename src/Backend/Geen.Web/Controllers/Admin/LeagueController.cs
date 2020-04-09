using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Leagues;
using Geen.Core.Domains.Leagues.Commands;
using Geen.Core.Domains.Leagues.Queries;
using Geen.Core.Interfaces.Common;
using Geen.Web.Application.Attributes;
using Geen.Web.Application.Authentication.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Geen.Web.Controllers.Admin
{
    [Route("api/admin/[controller]/[action]")]
    [AuthenticationFilter]
    public class LeagueController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public LeagueController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        [HttpGet]
        public Task<List<LeagueModel>> List()
        {
            return _queryDispatcher.Execute(new LeagueGetListQuery());
        }

        [HttpGet]
        public Task<LeagueModel> Get(int id)
        {
            return _queryDispatcher.Execute(new LeagueGetByIdQuery { Id = id });
        }

        [HttpPost]
        public Task Save([FromBody]LeagueModel obj)
        {
            return _commandDispatcher.Execute(new LeagueSaveCommand
            {
                Model = obj
            });
        }
    }
}
