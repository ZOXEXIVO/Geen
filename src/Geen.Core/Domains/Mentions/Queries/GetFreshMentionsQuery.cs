using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Interfaces.Common;
using Geen.Core.Utils;

namespace Geen.Core.Domains.Mentions.Queries
{
    public class GetFreshMentionsQuery : IQuery<Task<List<string>>>
    {
        public DateTime? DateStart { get; set; }
    }

    public class GetFreshMentionsQueryHandler : IQueryHandler<GetFreshMentionsQuery, Task<List<string>>>
    {
        private readonly IMentionRepository _mentionRepository;

        private readonly ConcurrentDictionary<string, long> _cache 
            = new ConcurrentDictionary<string, long>();

        public GetFreshMentionsQueryHandler(IMentionRepository mentionRepository)
        {
            _mentionRepository = mentionRepository;
        }

        public async Task<List<string>> Execute(GetFreshMentionsQuery query)
        {
            var mentionTask = _mentionRepository.GetFreshMentions(query.DateStart);
            var titledMentionTask = _mentionRepository.GetFreshTitledMentions(query.DateStart);
            var repliedMentionTask = _mentionRepository.GetFreshRepliedMentionIds(query.DateStart);

            await Task.WhenAll(mentionTask, titledMentionTask, repliedMentionTask);

            var fullMentions = mentionTask.Result
                .SelectMany(x => GetMentionUrls(x));

            var titleMentions = titledMentionTask.Result
                .SelectMany(x => GetMentionUrls(x, fromReply: false, forTitle: true));

            var fromReplyMentions = repliedMentionTask.Result
                .SelectMany(x => GetMentionUrls(x, fromReply: true));

            return fullMentions
                .Union(titleMentions)
                .Union(fromReplyMentions)
                .ToList();
        }

        private IEnumerable<string> GetMentionUrls(MentionModel mention, bool fromReply = false, bool forTitle = false)
        {
            if(mention.Club != null)
                return GetClubUrls(mention, fromReply, forTitle);

            return GetPlayerUrls(mention, fromReply, forTitle);
        }               

        private IEnumerable<string> GetClubUrls(MentionModel mention, bool fromReply, bool forTitle)
        {
            yield return "https://geen.one/";

            yield return $"https://geen.one/club/{mention.Club.UrlName}/{mention.Id}";

            if (forTitle)
                yield break;

            yield return $"https://geen.one/club/{mention.Club.UrlName}";
            
            if (!fromReply)
            {
                var clubMentionCount = _cache.GetOrAdd(mention.Club.UrlName, clubUrlName => 
                    _mentionRepository.GetClubItemsCount(clubUrlName));

                var clubPages = PagingExtensions.GetTotalPages(clubMentionCount);

                for (int clubPage = 2; clubPage < clubPages; clubPage++)
                {
                    yield return $"https://geen.one/club/{mention.Club.UrlName}/page/{clubPage}";
                }
            }           

            //----------------------

            var pages = PagingExtensions.GetTotalPages(mention.RepliesCount);

            for(int page = 2; page <= pages; page++)
            {
                yield return $"https://geen.one/club/{mention.Club.UrlName}/{mention.Id}/page/{page}";
            }
        }

        private IEnumerable<string> GetPlayerUrls(MentionModel mention, bool fromReply, bool forTitle)
        {            
            yield return $"https://geen.one/player/{mention.Player.UrlName}/{mention.Id}";

            if (forTitle)
                yield break;

            yield return $"https://geen.one/player/{mention.Player.UrlName}";

            if (!fromReply)
            {
                var playerMentionCount = _cache.GetOrAdd(mention.Player.UrlName, playerUrlName => 
                    _mentionRepository.GetClubItemsCount(playerUrlName));

                var playerPages = PagingExtensions.GetTotalPages(playerMentionCount);

                for (int playerPage = 2; playerPage < playerPages; playerPage++)
                {
                    yield return $"https://geen.one/player/{mention.Player.UrlName}/page/{playerPage}";
                }
            }                           

            //----------------------

            var pages = PagingExtensions.GetTotalPages(mention.RepliesCount);

            for (int page = 2; page <= pages; page++)
            {
                yield return $"https://geen.one/player/{mention.Player.UrlName}/{mention.Id}/page/{page}";
            }
        }
    }
}
