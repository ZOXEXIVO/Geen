using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Players;
using Geen.Core.Domains.Players.Commands;
using Geen.Core.Domains.Players.Queries;
using Geen.Core.Interfaces.Common;
using Geen.Web.Application.Attributes;
using Geen.Web.Application.Authentication.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Geen.Web.Controllers.Admin
{
    [Route("api/admin/[controller]/[action]")]
    [AuthenticationFilter]
    public class PlayerController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public PlayerController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        [HttpGet]
        public Task<List<PlayerModel>> List([FromJsonUri]PlayerGetListQuery query)
        {     
            return _queryDispatcher.Execute(query);
        }

        [HttpGet]
        public Task<PlayerModel> Get(int id)
        {
            return _queryDispatcher.Execute(new PlayerGetByIdQuery { Id = id });
        }

        [HttpPost]
        public Task Save([FromBody]PlayerModel obj)
        {
            obj.FirstName = obj.FirstName.Trim();
            obj.LastName = obj.LastName.Trim();
            obj.UrlName = obj.UrlName.Trim();
                        
            return _commandDispatcher.Execute(new PlayerSaveCommand
            {
                Model = obj
            });
        }

        [HttpGet]
        public Task<long> NextId()
        {
            return _queryDispatcher.Execute(new PlayerNextIdQuery());
        }
    }
}
