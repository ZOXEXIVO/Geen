using System.Threading.Tasks;
using Geen.Core.Domains.Mentions.Models;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Mentions.Commands
{
    public class MentionDislikeCommand : ICommand<Task>
    {
        public long Id { get; set; }
        public string UserId { get; set; }
    }

    public class MentionDislikeCommandDispatcher : ICommandDispatcher<MentionDislikeCommand, Task>
    {
        private readonly IMentionLikeRepository _mentionLikeRepository;
        private readonly IMentionRepository _mentionRepository;

        public MentionDislikeCommandDispatcher(IMentionLikeRepository mentionLikeRepository,
            IMentionRepository mentionRepository)
        {
            _mentionLikeRepository = mentionLikeRepository;
            _mentionRepository = mentionRepository;
        }

        public async Task Execute(MentionDislikeCommand command)
        {
            var redisResult = await GetRedisResult(command);

            await _mentionRepository.Dislike(command.Id, redisResult);
        }

        private async Task<IncrementStatus> GetRedisResult(MentionDislikeCommand command)
        {
            var status = await _mentionLikeRepository.GetLikeStatus(command.Id, command.UserId);

            var likesIncrement = 0;
            var dislikesIncrement = 0;

            if (!status.Dislikes && !status.Likes)
            {
                await _mentionLikeRepository.DislikeAddUser(command.Id, command.UserId);
                dislikesIncrement = 1;
            }
            else if (status.Dislikes && !status.Likes)
            {
                await _mentionLikeRepository.DislikeRemoveUser(command.Id, command.UserId);
                dislikesIncrement = -1;
            }
            else if (!status.Dislikes && status.Likes)
            {
                await Task.WhenAll(
                    _mentionLikeRepository.DislikeAddUser(command.Id, command.UserId),
                    _mentionLikeRepository.LikeRemoveUser(command.Id, command.UserId)
                );

                likesIncrement = -1;
                dislikesIncrement = 1;
            }

            return new IncrementStatus
            {
                Likes = likesIncrement,
                Dislikes = dislikesIncrement
            };
        }
    }
}
