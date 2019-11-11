using System;
using Geen.Data.Entities.Attributes;
using Geen.Data.Entities.Namespaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Geen.Data.Entities
{
    [MongoEntity("Votes", MongoNamespaces.Global)]
    public class VoteEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int? LeftPlayerId { get; set; }
        public int? RightPlayerId { get; set; }
        
        public int? WinnerId { get; set; }

        public DateTime Date {get;set;}
    }
}
