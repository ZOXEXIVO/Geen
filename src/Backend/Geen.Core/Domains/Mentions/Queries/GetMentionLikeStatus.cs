using System.Threading.Tasks;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Interfaces.Common;
using Geen.Core.Models.Likes;

namespace Geen.Core.Domains.Mentions.Queries;

public record GetMentionLikeStatus : IQuery<Task<LikeModel>>
{
    public long Id { get; set; }
}

public class GetLikeStatusQueryHandler : IQueryHandler<GetMentionLikeStatus, Task<LikeModel>>
{
    private readonly IMentionRepository _mentionRepository;

    public GetLikeStatusQueryHandler(IMentionRepository mentionRepository)
    {
        _mentionRepository = mentionRepository;
    }

    public Task<LikeModel> Execute(GetMentionLikeStatus query)
    {
        return _mentionRepository.GetLikeStatus(query.Id);
    }
}