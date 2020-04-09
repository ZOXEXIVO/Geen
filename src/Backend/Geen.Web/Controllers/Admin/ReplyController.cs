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

namespace Geen.Web.Controllers.Admin
{
    [Route("api/admin/[controller]/[action]")]
    [AuthenticationFilter]
    public class ReplyController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public ReplyController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        [HttpGet]
        public Task<List<ReplyModel>> UnapprovedList([FromJsonUri]GetReplyUnapprovedListQuery query)
        {
            return _queryDispatcher.Execute(query);
        }
        
        [HttpPost]
        public Task Approve(string id)
        {
            return _commandDispatcher.Execute(new ReplyApproveCommand
            {
                Id = id
            });
        }
        
        [HttpPost]
        public Task Disapprove(string id)
        {
            return _commandDispatcher.Execute(new ReplyDisapproveCommand
            {
                Id = id
            });
        }

        [HttpPost]
        public Task ChangeText(string id, [FromBody]BodyText text)
        {
            return _commandDispatcher.Execute(new ReplyChangeTextCommand
            {
                Id = id,
                Text = text.Text
            });
        }
        
        [HttpPost]
        public Task Remove(string id)
        {
            return _commandDispatcher.Execute(new ReplyRemoveCommand
            {
                ReplyId = id
            });
        }
    }
}
