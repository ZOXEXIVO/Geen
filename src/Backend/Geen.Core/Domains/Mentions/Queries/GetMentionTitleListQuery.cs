using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Mentions.Queries
{
    public class GetMentionTitleListQuery : IQuery<Task<List<MentionModel>>>
    {
        public long? Id { get; set; }
        public int Page { get; set; }
    }

    public class GetMentionTitleListQueryHandler : IQueryHandler<GetMentionTitleListQuery, Task<List<MentionModel>>>
    {
        private readonly IMentionRepository _mentionRepository;

        public GetMentionTitleListQueryHandler(IMentionRepository mentionRepository)
        {
            _mentionRepository = mentionRepository;
        }

        public Task<List<MentionModel>> Execute(GetMentionTitleListQuery query)
        {
            return _mentionRepository.GetMentionTitleList(query.Id, query.Page);
        }
    }
}
