using System;
using System.Collections.Generic;
using Geen.Data.Entities.Attributes;
using Geen.Data.Entities.Namespaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Geen.Data.Entities;

[MongoEntity("Mentions", MongoNamespaces.Global)]
public class MentionEntity
{
    public MentionEntity()
    {
        Related = new MentionRelatedEntity();
    }

    [BsonId] public long Id { get; set; }

    public string Title { get; set; }

    public string Text { get; set; }

    public DateTime Date { get; set; }

    public UserEntity User { get; set; }

    public PlayerEntity Player { get; set; }

    public ClubEntity Club { get; set; }

    public int Likes { get; set; }

    public int Dislikes { get; set; }

    public double[] Location { get; set; }

    public int RepliesCount { get; set; }

    public string SourceUrl { get; set; }

    public bool IsApproved { get; set; }

    public MentionRelatedEntity Related { get; set; }

    public DateTime? TitleChangeDate { get; set; }

    public bool ContainsUrlName(string urlName)
    {
        return Player?.UrlName == urlName || Club?.UrlName == urlName;
    }

    public class MentionRelatedEntity
    {
        public MentionRelatedEntity()
        {
            Players = new List<int>();
            Clubs = new List<int>();
        }

        public List<int> Players { get; set; }
        public List<int> Clubs { get; set; }
    }
}

#region Unwinded

public class MentionUnwinded
{
    public MentionUnwinded()
    {
        Related = new MentionRelatedUnwinded();
    }

    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public MentionRelatedUnwinded Related { get; set; }

    public class MentionRelatedUnwinded
    {
        public int Players { get; set; }
    }
}

#endregion