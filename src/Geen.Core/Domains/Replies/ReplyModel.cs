using System;
using Geen.Core.Domains.Users;

namespace Geen.Core.Domains.Replies
{
    public class ReplyModel
    {
        public string Id { get; set; }

        public long MentionId { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public UserModel User { get; set; }

        public bool IsApproved { get; set; }
    }
}
