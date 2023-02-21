using System;

namespace Geen.Data.Entities.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class MongoEntityAttribute : Attribute
{
    public MongoEntityAttribute(string collectionName, string schemaName = null)
    {
        CollectionName = collectionName;
        SchemaName = schemaName;
    }

    public string CollectionName { get; set; }
    public string SchemaName { get; set; }
}