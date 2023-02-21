using MongoDB.Bson.Serialization.Attributes;

namespace Geen.Data.Entities;

public class UserEntity
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string ProfileImage { get; set; }

    [BsonElement("IsAnon")] public bool? IsAnonymous { get; set; }
}