using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Players;
using Geen.Core.Domains.Search.Queries;
using Geen.Core.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;

namespace Geen.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public SearchController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("/api/search/{query}")]
        public Task<List<PlayerModel>> Get(string query)
        {
            return _queryDispatcher.Execute(new SearchQuery
            {
                Query = query,
                Count = 100
            });
        }
    }
}
