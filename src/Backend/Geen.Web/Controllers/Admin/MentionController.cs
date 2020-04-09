using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Mentions;
using Geen.Core.Domains.Mentions.Commands;
using Geen.Core.Domains.Mentions.Queries;
using Geen.Core.Interfaces.Common;
using Geen.Web.Application.Attributes;
using Geen.Web.Application.Authentication.Filters;
using Geen.Web.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Geen.Web.Controllers.Admin
{
    [Route("api/admin/[controller]/[action]")]
    [AuthenticationFilter]
    public class MentionController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public MentionController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        [HttpGet]
        public Task<List<MentionModel>> UnapprovedList([FromJsonUri]GetMentionListQuery query)
        {
            return _queryDispatcher.Execute(query);
        }

        [HttpGet]
        public Task<List<MentionModel>> TitlesList([FromJsonUri]GetMentionTitleListQuery query)
        {
            return _queryDispatcher.Execute(query);
        }

        [HttpPost]
        public Task Approve(long id)
        {
            return _commandDispatcher.Execute(new MentionApproveCommand
            {
                Id = id
            });
        }

        [HttpPost]
        public Task ChangeTitle(long id, string title)
        {
            return _commandDispatcher.Execute(new MentionChangeTitleCommand
            {
                Id = id,
                Title = title
            });
        }

        [HttpPost]
        public Task ChangeText(long id, [FromBody]BodyText text)
        {
            return _commandDispatcher.Execute(new MentionChangeTextCommand
            {
                Id = id,
                Text = text.Text
            });
        }

        [HttpPost]
        public Task Disapprove(long id)
        {
            return _commandDispatcher.Execute(new MentionDisapproveCommand
            {
                Id = id
            });
        }

        [HttpPost]
        public Task Remove(long id)
        {
            return _commandDispatcher.Execute(new MentionRemoveCommand
            {
                Id = id
            });
        }

        [HttpPost]
        public Task SetAnonymousAvatar(long id)
        {
            return _commandDispatcher.Execute(new MentionSetDefaultAvatarCommand
            {
                Id = id
            });
        }
    }
}
