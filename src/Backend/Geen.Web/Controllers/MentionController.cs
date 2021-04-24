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

        [HttpGet]
        [Route("/api/mention/{urlName}/{id}")]
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
        
        [HttpGet]
        [Route("/api/mention/list")]
        public Task<List<MentionModel>> GetList([FromJsonUri]GetMentionListQuery query)
        {
            query.IsApproved = true;
            
            return _queryDispatcher.Execute(query);
        }

        [HttpGet]
        [Route("/api/mention/fresh/{unixtime}")]
        public Task<List<string>> GetList(long unixtime)
        {
            return _queryDispatcher.Execute(new GetFreshMentionsQuery
            {
                DateStart = DateTimeOffset.FromUnixTimeSeconds(unixtime).DateTime
            });
        }

        [HttpPost]
        [ThrottleFilter(0, 0, 3, 0)]
        [Route("/api/mention/create")]
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

        [HttpPost]
        //[ThrottleFilter(0, 0, 1, 0)]
        [Route("/api/mention/like")]
        public async Task<LikeModel> Like(long id)
        {
            await _commandDispatcher.Execute(new MentionLikeCommand
            {
                Id = id,
                UserId = _userService.GetCurrentUser().Id
            });

            return await _queryDispatcher.Execute(new GetMentionLikeStatus { Id = id });
        }

        [HttpPost]
        //[ThrottleFilter(0, 0, 1, 0)]
        [Route("/api/mention/dislike")]
        public async Task<LikeModel> Dislike(long id)
        {
            await _commandDispatcher.Execute(new MentionDislikeCommand
            {
                Id = id,
                UserId = _userService.GetCurrentUser().Id
            });

            return await _queryDispatcher.Execute(new GetMentionLikeStatus { Id = id });
        }

        [HttpGet]
        [Route("/api/mention/init")]
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
