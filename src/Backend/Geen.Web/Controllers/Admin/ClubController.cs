using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Clubs;
using Geen.Core.Domains.Clubs.Commands;
using Geen.Core.Domains.Clubs.Queries;
using Geen.Core.Interfaces.Common;
using Geen.Web.Application.Authentication.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Geen.Web.Controllers.Admin
{
    [Route("api/admin/[controller]/[action]")]
    [AuthenticationFilter]
    public class ClubController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public ClubController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        [HttpGet]
        public Task<List<ClubModel>> List()
        {
            return _queryDispatcher.Execute(new ClubGetListQuery());
        }

        [HttpGet]
        public Task<ClubModel> Get(int id)
        {
            return _queryDispatcher.Execute(new ClubGetByIdQuery { Id = id });
        }

        [HttpPost]
        public Task Save([FromBody]ClubModel obj)
        {
            return _commandDispatcher.Execute(new ClubSaveCommand
            {
                Model = obj
            });
        }

        [HttpGet]
        public Task<long> NextId()
        {
            return _queryDispatcher.Execute(new ClubNextIdQuery());
        }
    }
}
