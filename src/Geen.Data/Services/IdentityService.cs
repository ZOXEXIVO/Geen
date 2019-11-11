using System.Threading.Tasks;
using Geen.Data.Entities;
using Geen.Data.Storages.Mongo;
using MongoDB.Driver;

public class IdentityService
{
    private readonly MongoContext _context;

    public IdentityService(MongoContext context)
    {
        _context = context;
    }

    public async Task<long> Next<T>()
    {
        var typename = typeof(T).Name;

        var filter = Builders<IdentityEntity>.Filter.Eq(x => x.Name, typename);

        var updateDefinition = Builders<IdentityEntity>.Update.Inc(x => x.Value, 1);

        var options = new FindOneAndUpdateOptions<IdentityEntity>
        {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };

        var updateResult = await _context.For<IdentityEntity>()
            .FindOneAndUpdateAsync(filter, updateDefinition, options);

        return updateResult.Value;
    }
}