using System.Threading.Tasks;
using Geen.Core.Domains.Votes;
using Geen.Core.Domains.Votes.Repositories;
using Geen.Data.Entities;
using Geen.Data.Extensions;
using Geen.Data.Storages.Mongo;

namespace Geen.Data.Repositories;

public class VoteRepository(MongoContext context) : IVoteRepository
{
    public Task Create(VoteModel model)
    {
        var entity = model.Map<VoteEntity>();

        return context.For<VoteEntity>().InsertOneAsync(entity);
    }
}