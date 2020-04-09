using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Mentions.Queries
{
    public class GetMentionIdentitiesQuery : IQuery<Task<List<MentionModel>>>
    {
    }

    public class GetMentionIdentitiesQueryHandler : IQueryHandler<GetMentionIdentitiesQuery, Task<List<MentionModel>>>
    {
        private readonly IMentionRepository _mentionRepository;

        public GetMentionIdentitiesQueryHandler(IMentionRepository mentionRepository)
        {
            _mentionRepository = mentionRepository;
        }

        public Task<List<MentionModel>> Execute(GetMentionIdentitiesQuery query)
        {
            return _mentionRepository.GetMentionIdentities();
        }
    }
}
