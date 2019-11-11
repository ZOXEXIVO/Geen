using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Countries;
using Geen.Core.Domains.Countries.Repositories;
using Geen.Data.Entities;
using Geen.Data.Extensions;
using Geen.Data.Storages.Mongo;
using MongoDB.Driver;

namespace Geen.Data.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly MongoContext _context;

        public CountryRepository(MongoContext context)
        {
            _context = context;
        }

        public async Task<CountryModel> GetById(int id)
        {
            var result = await _context.For<CountryEntity>()
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return result.Map<CountryModel>();
        }

        public async Task<CountryModel> GetByUrlName(string urlName)
        {
            var result = await _context.For<CountryEntity>()
                .Find(x => x.UrlName == urlName)
                .FirstOrDefaultAsync();

            return result.Map<CountryModel>();
        }

        public async Task<List<CountryModel>> GetAll()
        {
            var result = await _context.For<CountryEntity>()
                .Find(x => true)
                .ToListAsync();

            return result.Map<List<CountryModel>>();
        }

        public async Task<long> GetNextId()
        {
            var projection = Builders<CountryEntity>.Projection.Expression(x => (int?)x.Id);

            var lastId = await _context.For<CountryEntity>()
                .Find(x => true)
                .SortByDescending(x => x.Id)
                .Project(projection)
                .FirstOrDefaultAsync();

            if (!lastId.HasValue)
                return 1;

            return lastId.Value + 1;
        }

        public Task Save(CountryModel model)
        {
            var entity = model.Map<CountryEntity>();

            return _context.For<CountryEntity>()
                .ReplaceOneAsync(x => x.Id == model.Id, entity,
                    new UpdateOptions { IsUpsert = true });
        }
    }
}
