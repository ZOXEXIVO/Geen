using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Clubs;
using Geen.Core.Domains.Clubs.Repositories;
using Geen.Data.Entities;
using Geen.Data.Extensions;
using Geen.Data.Storages.Mongo;
using MongoDB.Driver;

namespace Geen.Data.Repositories
{
    public class ClubRepository : IClubRepository
    {
        private readonly MongoContext _context;

        public ClubRepository(MongoContext context)
        {
            _context = context;
        }

        public async Task<ClubModel> GetById(int id)
        {
            var result = await _context.For<ClubEntity>()
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return result.Map<ClubModel>();
        }

        public async Task<ClubModel> GetByUrlName(string urlName)
        {
            var result = await _context.For<ClubEntity>()
                .Find(x => x.UrlName == urlName)
                .FirstOrDefaultAsync();

            return result.Map<ClubModel>();
        }

        public async Task<List<ClubModel>> GetAll()
        {
            var result = await _context.For<ClubEntity>()
                .Find(x => true)
                .ToListAsync();

            return result.Map<List<ClubModel>>();
        }

        public async Task<List<ClubModel>> GetAllUrls()
        {
            var projection = Builders<ClubEntity>
                .Projection
                .Include(x => x.UrlName)
                .Include(x => x.IsNational);

            var result = await _context.For<ClubEntity>()
                .Find(x => true)
                .Project<ClubEntity>(projection)
                .ToListAsync();

            return result.Map<List<ClubModel>>();
        }

        public async Task<long> GetNextId()
        {
            var projection = Builders<ClubEntity>
                .Projection
                .Expression(x => (int?)x.Id);

            var lastId = await _context.For<ClubEntity>()
                .Find(x => true)
                .SortByDescending(x => x.Id)
                .Project(projection)
                .FirstOrDefaultAsync();

            if (!lastId.HasValue)
                return 1;

            return lastId.Value + 1;
        }

        public Task<List<DateTime>> GetBirthdays(string urlName)
        {
            var projection = Builders<PlayerEntity>
                .Projection
                .Expression(x => x.BirthDate);

            return _context.For<PlayerEntity>()
                .Find(x => x.Club.UrlName == urlName && x.Position != 4) //no coach
                .Project(projection)
                .ToListAsync();
        }

        public Task Save(ClubModel model)
        {
            var entity = model.Map<ClubEntity>();

            return _context.For<ClubEntity>()
                .ReplaceOneAsync(x => x.Id == model.Id, entity,
                    new ReplaceOptions { IsUpsert = true });
        }

        public async Task<List<ClubModel>> GetCached()
        {
            var projection = Builders<ClubEntity>.Projection
                .Include(x => x.Id)
                .Include(x => x.Name)
                .Include(x => x.UrlName);
            
            var result = await _context.For<ClubEntity>()
                .Find(x => true)
                .Project<ClubEntity>(projection)
                .ToListAsync();

            return result.Map<List<ClubModel>>();
        }
    }
}
