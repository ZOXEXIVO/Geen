using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Clubs;
using Geen.Core.Domains.Clubs.Commands;
using Geen.Core.Domains.Clubs.Queries;
using Geen.Core.Interfaces.Common;
using Geen.Web.Application.Authentication.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Geen.Web.Controllers.Areas.Admin;

[AuthenticationFilter]
public class AdminClubController : Controller
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public AdminClubController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    [HttpGet("api/admin/club", Name = "getAdminClubList")]
    public Task<List<ClubModel>> List()
    {
        return _queryDispatcher.Execute(new ClubGetListQuery());
    }

    [HttpGet("api/admin/club/{id:int}", Name = "getAdminClub")]
    public Task<ClubModel> Get(int id)
    {
        return _queryDispatcher.Execute(new ClubGetByIdQuery { Id = id });
    }

    [HttpPost("api/admin/club", Name = "saveAdminClub")]
    public Task Save([FromBody] ClubModel obj)
    {
        return _commandDispatcher.Execute(new ClubSaveCommand
        {
            Model = obj
        });
    }

    [HttpGet("api/admin/club/next-id", Name = "getAdminClubNextId")]
    public Task<long> NextId()
    {
        return _queryDispatcher.Execute(new ClubNextIdQuery());
    }
}