using System.Threading.Tasks;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Models.Likes;
using Geen.Data.Storages.Redis;

namespace Geen.Data.Repositories.Likes;

public class MentionLikeRepository : IMentionLikeRepository
{
    private readonly RedisStore _redisStore;

    public MentionLikeRepository(RedisStore redisStore)
    {
        _redisStore = redisStore;
    }

    public async Task<LikeStatus> GetLikeStatus(long id, string userId)
    {
        var likesStatus = _redisStore.Current.SetContainsAsync(GetLikeKey(id), userId);
        var dislikesStatus = _redisStore.Current.SetContainsAsync(GetDislikeKey(id), userId);

        await Task.WhenAll(likesStatus, dislikesStatus);

        return new LikeStatus
        {
            Likes = likesStatus.Result,
            Dislikes = dislikesStatus.Result
        };
    }

    public Task LikeAddUser(long id, string userId)
    {
        return _redisStore.Current.SetAddAsync(GetLikeKey(id), userId);
    }

    public Task LikeRemoveUser(long id, string userId)
    {
        return _redisStore.Current.SetRemoveAsync(GetLikeKey(id), userId);
    }

    public Task DislikeAddUser(long id, string userId)
    {
        return _redisStore.Current.SetAddAsync(GetDislikeKey(id), userId);
    }

    public Task DislikeRemoveUser(long id, string userId)
    {
        return _redisStore.Current.SetRemoveAsync(GetDislikeKey(id), userId);
    }

    private string GetLikeKey(long id)
    {
        return $"MENTION_{id}_LIKES";
    }

    private string GetDislikeKey(long id)
    {
        return $"MENTION_{id}_DISLIKES";
    }
}