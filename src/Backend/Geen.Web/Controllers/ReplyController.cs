using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Replies;
using Geen.Core.Domains.Replies.Commands;
using Geen.Core.Domains.Replies.Queries;
using Geen.Core.Interfaces.Common;
using Geen.Web.Application.Authentication.Services;
using Geen.Web.Application.Filters.Throttling;
using Geen.Web.Application.Formatter.Bindings;
using Geen.Web.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Geen.Web.Controllers
{
    public class ReplyController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        private readonly UserService _userService;

        public ReplyController(UserService userService,
            IQueryDispatcher queryDispatcher,
            ICommandDispatcher commandDispatcher)
        {
            _userService = userService;
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        [HttpGet("/api/reply")]
        public Task<List<ReplyModel>> GetList([FromJsonUri] GetReplyListQuery query)
        {
            return _queryDispatcher.Execute(query);
        }

        [HttpGet("/api/reply/latest")]
        public Task<List<ReplyModel>> GetLatest([FromJsonUri] GetReplyLatestQuery query)
        {
            return _queryDispatcher.Execute(query);
        }

        [HttpPost("/api/reply/create", Name = "createReply")]
        [ThrottleFilter(0, 0, 3, 0)]
        public async Task<ReplyModel> Create([FromBody] ReplyCreateDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Text))
                return null;

            return await _commandDispatcher.Execute(new ReplyCreateCommand
            {
                Model = request.ToModel(),
                User = _userService.GetCurrentUser()
            });
        }
    }
}