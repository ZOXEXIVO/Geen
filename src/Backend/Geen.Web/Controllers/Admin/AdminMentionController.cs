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
    [AuthenticationFilter]
    public class AdminMentionController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public AdminMentionController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        [HttpGet("api/admin/mention/unapproved")]
        public Task<List<MentionModel>> UnapprovedList([FromJsonUri]GetMentionListQuery query)
        {
            return _queryDispatcher.Execute(query);
        }

        [HttpGet("api/admin/mention/titles")]
        public Task<List<MentionModel>> TitlesList([FromJsonUri]GetMentionTitleListQuery query)
        {
            return _queryDispatcher.Execute(query);
        }

        [HttpPost("api/admin/mention/approve")]
        public Task Approve(long id)
        {
            return _commandDispatcher.Execute(new MentionApproveCommand
            {
                Id = id
            });
        }

        [HttpPut("api/admin/mention/title")]
        public Task ChangeTitle(long id, string title)
        {
            return _commandDispatcher.Execute(new MentionChangeTitleCommand
            {
                Id = id,
                Title = title
            });
        }

        [HttpPut("api/admin/mention/text")]
        public Task ChangeText(long id, [FromBody]BodyText text)
        {
            return _commandDispatcher.Execute(new MentionChangeTextCommand
            {
                Id = id,
                Text = text.Text
            });
        }

        [HttpPost("api/admin/mention/disapprove")]
        public Task Disapprove(long id)
        {
            return _commandDispatcher.Execute(new MentionDisapproveCommand
            {
                Id = id
            });
        }

        [HttpDelete("api/admin/mention/{id:int}", Name = "mentionDelete")]
        public Task Remove(long id)
        {
            return _commandDispatcher.Execute(new MentionRemoveCommand
            {
                Id = id
            });
        }

        [HttpPut("api/admin/mention/anonymous-avatar")]
        public Task SetAnonymousAvatar(long id)
        {
            return _commandDispatcher.Execute(new MentionSetDefaultAvatarCommand
            {
                Id = id
            });
        }
    }
}
