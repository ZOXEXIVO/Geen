using System.Threading.Tasks;
using Geen.Core.Domains.Votes;
using Geen.Core.Domains.Votes.Commands;
using Geen.Core.Domains.Votes.Queries;
using Geen.Core.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;

namespace Geen.Web.Controllers;

public class VotingController : Controller
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public VotingController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    [HttpGet("/api/votes")]
    public Task<VoteFullModel> GetClubVotingModel()
    {
        return _queryDispatcher.Execute(new GetVoteDataQuery());
    }

    [HttpPost("/api/vote")]
    public Task Vote([FromBody] CreateVoteCommand command)
    {
        return _commandDispatcher.Execute(command);
    }
}