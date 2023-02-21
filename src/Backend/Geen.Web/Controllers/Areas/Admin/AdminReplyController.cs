using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Replies;
using Geen.Core.Domains.Replies.Commands;
using Geen.Core.Domains.Replies.Queries;
using Geen.Core.Interfaces.Common;
using Geen.Web.Application.Attributes;
using Geen.Web.Application.Authentication.Filters;
using Geen.Web.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Geen.Web.Controllers.Areas.Admin;

[AuthenticationFilter]
public class AdminReplyController : Controller
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public AdminReplyController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    [HttpGet("api/admin/reply", Name = "unapproved")]
    public Task<List<ReplyModel>> UnapprovedList([FromJsonUri] GetReplyUnapprovedListQuery query)
    {
        return _queryDispatcher.Execute(query);
    }

    [HttpPatch("api/admin/reply/approve", Name = "approveReply")]
    public Task Approve(string id)
    {
        return _commandDispatcher.Execute(new ReplyApproveCommand
        {
            Id = id
        });
    }

    [HttpPatch("api/admin/reply/disapprove", Name = "disapproveReply")]
    public Task Disapprove(string id)
    {
        return _commandDispatcher.Execute(new ReplyDisapproveCommand
        {
            Id = id
        });
    }

    [HttpPut("api/admin/reply/text")]
    public Task ChangeText(string id, [FromBody] BodyText text)
    {
        return _commandDispatcher.Execute(new ReplyChangeTextCommand
        {
            Id = id,
            Text = text.Text
        });
    }

    [HttpDelete("api/admin/reply/{id}", Name = "removeReply")]
    public Task Remove(string id)
    {
        return _commandDispatcher.Execute(new ReplyRemoveCommand
        {
            ReplyId = id
        });
    }
}