using System;
using System.Collections.Generic;
using Geen.Core.Domains.Clubs;
using Geen.Core.Domains.Players;
using Geen.Core.Domains.Users;

namespace Geen.Core.Domains.Mentions;

public class MentionModel
{
    public MentionModel()
    {
        Related = new MentionRelatedModel();
    }

    public long Id { get; set; }

    public string Title { get; set; }

    public string Text { get; set; }

    public DateTime Date { get; set; }

    public UserModel User { get; set; }

    public PlayerModel Player { get; set; }

    public ClubModel Club { get; set; }

    public int Likes { get; set; }

    public int Dislikes { get; set; }

    public double[] Location { get; set; }

    public int RepliesCount { get; set; }

    public string SourceUrl { get; set; }

    public bool IsApproved { get; set; }

    public MentionRelatedModel Related { get; set; }

    public DateTime? TitleChangeDate { get; set; }

    public bool ContainsUrlName(string urlName)
    {
        return Player?.UrlName == urlName || Club?.UrlName == urlName;
    }

    public class MentionRelatedModel
    {
        public MentionRelatedModel()
        {
            Players = new List<int>();
            Clubs = new List<int>();
        }

        public List<int> Players { get; set; }
        public List<int> Clubs { get; set; }
    }
}