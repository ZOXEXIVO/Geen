using System.Threading.Tasks;
using Geen.Core.Domains.Replies.Repositories;
using Geen.Core.Models.Likes;
using Geen.Data.Storages.Redis;

namespace Geen.Data.Repositories.Likes
{
    public class ReplyLikeRepository : IReplyLikeRepository
    {
        private readonly RedisStore _redisStore;

        public ReplyLikeRepository(RedisStore redisStore)
        {
            _redisStore = redisStore;
        }

        private string GetLikeKey(string id)
        {
            return $"REPLY_{id}_LIKES";
        }

        private string GetDislikeKey(string id)
        {
            return $"REPLY_{id}_DISLIKES";
        }

        public async Task<LikeStatus> GetLikeStatus(string id, string userId)
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

        public Task LikeAddUser(string id, string userId)
        {
            return _redisStore.Current.SetAddAsync(GetLikeKey(id), userId);
        }

        public Task LikeRemoveUser(string id, string userId)
        {
            return _redisStore.Current.SetRemoveAsync(GetLikeKey(id), userId);
        }

        public Task DislikeAddUser(string id, string userId)
        {
            return _redisStore.Current.SetAddAsync(GetDislikeKey(id), userId);
        }

        public Task DislikeRemoveUser(string id, string userId)
        {
            return _redisStore.Current.SetRemoveAsync(GetDislikeKey(id), userId);
        }
    }
}
