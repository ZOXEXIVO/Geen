using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Countries;
using Geen.Core.Domains.Countries.Commands;
using Geen.Core.Domains.Countries.Queries;
using Geen.Core.Interfaces.Common;
using Geen.Web.Application.Authentication.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Geen.Web.Controllers.Admin
{
    [Route("api/admin/[controller]/[action]")]
    [AuthenticationFilter]
    public class CountryController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public CountryController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        [HttpGet]
        public Task<List<CountryModel>> List()
        {
            return _queryDispatcher.Execute(new CountryGetListQuery());
        }

        [HttpGet]
        public Task<CountryModel> Get(int id)
        {
            return _queryDispatcher.Execute(new CountryGetByIdQuery { Id = id });
        }

        [HttpPost]
        public Task Save([FromBody]CountryModel obj)
        {
            return _commandDispatcher.Execute(new CountrySaveCommand
            {
                Model = obj
            });
        }
    }
}
