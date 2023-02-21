using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Countries;
using Geen.Core.Domains.Countries.Commands;
using Geen.Core.Domains.Countries.Queries;
using Geen.Core.Interfaces.Common;
using Geen.Web.Application.Authentication.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Geen.Web.Controllers.Areas.Admin;

[AuthenticationFilter]
public class AdminCountryController : Controller
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public AdminCountryController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    [HttpGet("api/admin/country", Name = "getAdminCountryList")]
    public Task<List<CountryModel>> List()
    {
        return _queryDispatcher.Execute(new CountryGetListQuery());
    }

    [HttpGet("api/admin/country/{id:int}", Name = "getAdminCountry")]
    public Task<CountryModel> Get(int id)
    {
        return _queryDispatcher.Execute(new CountryGetByIdQuery { Id = id });
    }

    [HttpPost("api/admin/country", Name = "saveAdminCountry")]
    public Task Save([FromBody] CountryModel obj)
    {
        return _commandDispatcher.Execute(new CountrySaveCommand
        {
            Model = obj
        });
    }
}