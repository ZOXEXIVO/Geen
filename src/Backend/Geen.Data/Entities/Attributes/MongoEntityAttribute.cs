using System;

namespace Geen.Data.Entities.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MongoEntityAttribute : Attribute
    {
        public string CollectionName { get; set; }
        public string SchemaName { get; set; }

        public MongoEntityAttribute(string collectionName, string schemaName = null)
        {
            CollectionName = collectionName;
            SchemaName = schemaName;
        }
    }
}
