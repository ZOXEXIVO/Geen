using Geen.Core.Interfaces.Common;
using Geen.Core.Models.Likes;
using Geen.Web.Application.Attributes;
using Geen.Web.Application.Authentication.Services;
using Geen.Web.Application.Filters.Throttling;
using Geen.Web.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Mentions;
using Geen.Core.Domains.Mentions.Commands;
using Geen.Core.Domains.Mentions.Queries;

namespace Geen.Web.Controllers
{
    public class MentionController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        private readonly UserService _userService;

        public MentionController(IQueryDispatcher queryDispatcher, 
            UserService userService,
            ICommandDispatcher commandDispatcher)
        {
            _userService = userService;
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("/api/mention/{urlName}/{id:long}", Name = "getMention")]
        public async Task<MentionModel> Get(string urlName, long id)
        {
            var result = await _queryDispatcher.Execute(new GetMentionByIdQuery
            {
                Id = id
            });

            if (result == null || !result.ContainsUrlName(urlName))
                return null;

            return result;
        }
        
        [HttpGet("/api/mention/list", Name = "getMentionList")]
        public Task<List<MentionModel>> GetList([FromJsonUri]GetMentionListQuery query)
        {
            query.IsApproved = true;
            
            return _queryDispatcher.Execute(query);
        }

        [HttpGet("/api/mention/fresh/{unixtime:long}", Name = "fresh")]
        public Task<List<string>> GetList(long unixtime)
        {
            return _queryDispatcher.Execute(new GetFreshMentionsQuery
            {
                DateStart = DateTimeOffset.FromUnixTimeSeconds(unixtime).DateTime
            });
        }

        [HttpPost("/api/mention/create", Name = "create")]
        [ThrottleFilter(0, 0, 3, 0)]
        public async Task<MentionModel> Create([FromBody]MentionCreateDto request)
        { 
            if (string.IsNullOrWhiteSpace(request.Text))
                return null;

            var result = await _commandDispatcher.Execute(new MentionCreateCommand
            {
                Model = request.ToModel(),
                User = _userService.GetCurrentUser()
            });

            return result;
        }

        [HttpPost("/api/mention/like")]
        //[ThrottleFilter(0, 0, 1, 0)]
        public async Task<LikeModel> Like(long id)
        {
            await _commandDispatcher.Execute(new MentionLikeCommand
            {
                Id = id,
                UserId = _userService.GetCurrentUser().Id
            });

            return await _queryDispatcher.Execute(new GetMentionLikeStatus { Id = id });
        }

        [HttpPost("/api/mention/dislike")]
        //[ThrottleFilter(0, 0, 1, 0)]
        public async Task<LikeModel> Dislike(long id)
        {
            await _commandDispatcher.Execute(new MentionDislikeCommand
            {
                Id = id,
                UserId = _userService.GetCurrentUser().Id
            });

            return await _queryDispatcher.Execute(new GetMentionLikeStatus { Id = id });
        }

        [HttpGet("/api/mention/init")]
        public Task Init()
        {
            var cmd = new MentionRandomLikerCommand
            {
                Count = 5,
                DaysInteval = 20,
                MaxLikes = 15,
                MaxDislikes = 7
            };

            return Task.WhenAll(_commandDispatcher.Execute(cmd));
        }
    }
}
