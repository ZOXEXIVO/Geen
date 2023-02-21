using System.Threading.Tasks;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Mentions.Queries;

public record GetMentionByIdQuery : IQuery<Task<MentionModel>>
{
    public long Id { get; set; }
}

public class GetMentionByIdentityQueryHandler : IQueryHandler<GetMentionByIdQuery, Task<MentionModel>>
{
    private readonly IMentionRepository _mentionRepository;

    public GetMentionByIdentityQueryHandler(IMentionRepository mentionRepository)
    {
        _mentionRepository = mentionRepository;
    }

    public Task<MentionModel> Execute(GetMentionByIdQuery query)
    {
        return _mentionRepository.GetById(query.Id);
    }
}