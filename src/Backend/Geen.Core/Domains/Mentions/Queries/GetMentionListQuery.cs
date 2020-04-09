using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Mentions.Queries
{
    public class GetMentionListQuery : IQuery<Task<List<MentionModel>>>
    {
        public int? PlayerId { get; set; }

        public string ClubUrlName { get; set; }
        public string PlayerClubUrlName { get; set; }

        public string UserId { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public bool? IsApproved { get; set; }

        public bool? ForPlayer { get; set; }
        public bool? ForClub { get; set; }

        public bool Cached { get; set; }
        
        public int Page { get; set; }
    }

    public class GetMentionListQueryHandler : IQueryHandler<GetMentionListQuery, Task<List<MentionModel>>>
    {
        private readonly IMentionRepository _mentionRepository;

        public GetMentionListQueryHandler(IMentionRepository mentionRepository)
        {
            _mentionRepository = mentionRepository;
        }

        public Task<List<MentionModel>> Execute(GetMentionListQuery query)
        {
            return _mentionRepository.GetList(query);
        }
    }
}
