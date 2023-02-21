using System;
using Geen.Data.Entities.Attributes;
using Geen.Data.Entities.Namespaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Geen.Data.Entities;

[MongoEntity("Replies", MongoNamespaces.Global)]
public class ReplyEntity
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public long MentionId { get; set; }

    public string Text { get; set; }

    public DateTime Date { get; set; }

    public UserEntity User { get; set; }

    public bool IsApproved { get; set; }
}