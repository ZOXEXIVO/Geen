using Geen.Data.Entities.Attributes;
using Geen.Data.Entities.Namespaces;
using MongoDB.Bson.Serialization.Attributes;

namespace Geen.Data.Entities;

[MongoEntity("Identities", MongoNamespaces.Global)]
public class IdentityEntity
{
    [BsonId] public string Name { get; set; }

    public long Value { get; set; }
}