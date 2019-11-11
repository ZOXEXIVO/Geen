using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Leagues;
using Geen.Core.Domains.Leagues.Repositories;
using Geen.Data.Entities;
using Geen.Data.Extensions;
using Geen.Data.Storages.Mongo;
using Mapster;
using MongoDB.Driver;

namespace Geen.Data.Repositories
{
    public class LeagueRepository : ILeagueRepository
    {
        private readonly MongoContext _context;

        public LeagueRepository(MongoContext context)
        {
            _context = context;
        }

        public async Task<LeagueModel> GetById(int id)
        {
            var result = await _context.For<LeagueEntity>()
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return result.Map<LeagueModel>();
        }

        public async Task<LeagueModel> GetByUrlName(string urlName)
        {
            var result = await _context.For<LeagueEntity>()
                .Find(x => x.UrlName == urlName)
                .FirstOrDefaultAsync();

            return result.Map<LeagueModel>();
        }

        public async Task<List<LeagueModel>> GetAll()
        {
            var result = await _context.For<LeagueEntity>()
                .Find(x => true)
                .ToListAsync();

            return result.Map<List<LeagueModel>>();
        }

        public async Task<long> GetNextId()
        {
            var projection = Builders<LeagueEntity>.Projection.Expression(x => (int?)x.Id);

            var lastId = await _context.For<LeagueEntity>()
                .Find(x => true)
                .SortByDescending(x => x.Id)
                .Project(projection)
                .FirstOrDefaultAsync();

            if (!lastId.HasValue)
                return 1;

            return lastId.Value + 1;
        }

        public Task Save(LeagueModel model)
        {
            var entity = model.Map<LeagueEntity>();

            return _context.For<LeagueEntity>()
                .ReplaceOneAsync(x => x.Id == model.Id, entity,
                    new UpdateOptions { IsUpsert = true });
        }
    }
}
