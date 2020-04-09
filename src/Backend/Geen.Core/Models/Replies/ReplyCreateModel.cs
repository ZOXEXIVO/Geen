using Geen.Core.Domains.Users;

namespace Geen.Core.Models.Replies
{
    public class ReplyCreateModel
    {
        public long MentionId { get; set; }

        public UserModel User { get; set; }

        public string Text { get; set; }
    }
}
