using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Geen.Core.Domains.Players;
using Geen.Core.Domains.Players.Repositories;
using Geen.Data.Entities;
using Geen.Data.Extensions;
using Geen.Data.Storages.Mongo;
using Geen.Data.Utils;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Geen.Data.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly MongoContext _context;

    public PlayerRepository(MongoContext context)
    {
        _context = context;
    }

    public async Task<PlayerModel> GetById(int id)
    {
        var result = await _context.For<PlayerEntity>()
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync();

        return result.Map<PlayerModel>();
    }

    public async Task<PlayerModel> GetByUrlName(string urlName)
    {
        var result = await _context.For<PlayerEntity>()
            .Find(x => x.UrlName == urlName)
            .FirstOrDefaultAsync();

        return result.Map<PlayerModel>();
    }

    public async Task<PlayerModel> GetClubCoach(string clubUrlName)
    {
        const int coachPosition = 4;

        var result = await _context.For<PlayerEntity>()
            .Find(x => x.Club.UrlName == clubUrlName && x.Position == coachPosition)
            .FirstOrDefaultAsync();

        return result.Map<PlayerModel>();
    }

    public async Task<List<PlayerModel>> GetList(string query, string clubUrlName, int page)
    {
        var filter = FilterDefinition<PlayerEntity>.Empty;

        if (!string.IsNullOrWhiteSpace(clubUrlName))
            filter = filter & Builders<PlayerEntity>.Filter.Eq(x => x.Club.UrlName, clubUrlName);

        if (!string.IsNullOrWhiteSpace(query))
            filter = filter & Builders<PlayerEntity>.Filter.Regex(x => x.LastName,
                new BsonRegularExpression(Regex.Escape(query), "i"));

        var result = await _context.For<PlayerEntity>()
            .Find(filter)
            .SortByDescending(x => x.Id)
            .ToPagedAsync(page);

        return result.Map<List<PlayerModel>>();
    }

    public Task<List<string>> GetUrls()
    {
        var projection = Builders<PlayerEntity>
            .Projection
            .Expression(x => x.UrlName);

        return _context.For<PlayerEntity>()
            .Find(x => true)
            .Project(projection)
            .ToListAsync();
    }

    public async Task<List<PlayerModel>> GetByClubUrlName(string clubUrlName)
    {
        var result = await _context.For<PlayerEntity>()
            .Find(x => x.Club.UrlName == clubUrlName)
            .ToListAsync();

        return result.Map<List<PlayerModel>>();
    }

    public Task<List<int>> GetIdsByClubAndPosition(string clubUrlName, int position)
    {
        var projection = Builders<PlayerEntity>
            .Projection
            .Expression(x => x.Id);

        return _context.For<PlayerEntity>()
            .Find(x => x.Club.UrlName == clubUrlName && x.Position == position)
            .Project(projection)
            .ToListAsync();
    }

    public async Task<List<PlayerModel>> GetByIds(List<int> ids)
    {
        var result = await _context.For<PlayerEntity>()
            .Find(x => ids.Contains(x.Id))
            .ToListAsync();

        return result.Map<List<PlayerModel>>();
    }

    public Task<List<PlayerModel>> GetTopPlayers(string clubUrlName)
    {
        if (!string.IsNullOrWhiteSpace(clubUrlName))
            return GetTopClubPlayerModels(clubUrlName);

        return GetTopGlobalPlayerModels();
    }

    public async Task<List<PlayerModel>> GetRelatedPlayers(string urlName)
    {
        var filter = Builders<MentionEntity>.Filter
            .Where(x => x.Player.UrlName == urlName);

        var projection = Builders<MentionEntity>.Projection.Include(x => x.Related.Players);

        var topRelatedPlayerModels = await _context.For<MentionEntity>()
            .Aggregate(new AggregateOptions { AllowDiskUse = true })
            .Match(filter)
            .Project<MentionEntity>(projection)
            .Unwind<MentionEntity, MentionUnwinded>(x => x.Related.Players)
            .SortByCount(x => x.Related)
            .Limit(3)
            .ToListAsync();

        var topRelatedPlayerModelsIds = topRelatedPlayerModels
            .Select(x => x.Id.Players)
            .ToList();

        var result = await _context.For<PlayerEntity>()
            .Find(x => topRelatedPlayerModelsIds.Contains(x.Id))
            .ToListAsync();

        return result.Map<List<PlayerModel>>();
    }

    public async Task<PlayerModel> GetRandom()
    {
        var result = await _context.For<PlayerEntity>()
            .Aggregate()
            .Match(x => x.Club.UrlName != null)
            .AppendStage<PlayerEntity>("{ $sample: { size: 1 } }")
            .FirstOrDefaultAsync();

        return result.Map<PlayerModel>();
    }

    public async Task<(PlayerModel Left, PlayerModel Right)> GetForVotes(int position)
    {
        var leftPlayer = await _context.For<PlayerEntity>()
            .Aggregate()
            .Match(x => x.Position == position)
            .AppendStage<PlayerEntity>("{ $sample: { size: 1 } }")
            .FirstOrDefaultAsync();

        var rightPlayer = await _context.For<PlayerEntity>()
            .Aggregate()
            .Match(x => x.Id != leftPlayer.Id && x.Position == position)
            .AppendStage<PlayerEntity>("{ $sample: { size: 1 } }")
            .FirstOrDefaultAsync();

        return (leftPlayer.Map<PlayerModel>(), rightPlayer.Map<PlayerModel>());
    }

    public async Task<List<PlayerModel>> GetAll()
    {
        var result = await _context.For<PlayerEntity>()
            .Find(x => true)
            .ToListAsync();

        return result.Map<List<PlayerModel>>();
    }

    public async Task<long> GetNextId()
    {
        var projection = Builders<PlayerEntity>.Projection.Expression(x => (int?)x.Id);

        var lastId = await _context.For<PlayerEntity>()
            .Find(x => true)
            .SortByDescending(x => x.Id)
            .Project(projection)
            .FirstOrDefaultAsync();

        if (!lastId.HasValue)
            return 1;

        return lastId.Value + 1;
    }

    public Task IncrementMentionsCount(int id)
    {
        var update = Builders<PlayerEntity>.Update
            .Inc(x => x.MentionsCount, 1);

        return _context.For<PlayerEntity>()
            .UpdateOneAsync(x => x.Id == id, update);
    }

    public Task Save(PlayerModel model)
    {
        var entity = model.Map<PlayerEntity>();

        return _context.For<PlayerEntity>()
            .ReplaceOneAsync(x => x.Id == model.Id, entity,
                new ReplaceOptions { IsUpsert = true });
    }

    public async Task<List<PlayerModel>> Search(string query, int count)
    {
        var filter = Builders<PlayerEntity>.Filter
            .Regex(x => x.LastName, new BsonRegularExpression(Regex.Escape(query), "i"));

        var result = await _context.For<PlayerEntity>()
            .Find(filter)
            .Limit(count)
            .ToListAsync();

        return result.Map<List<PlayerModel>>();
    }

    public async Task<List<PlayerModel>> GetCached()
    {
        var projection = Builders<PlayerEntity>.Projection
            .Include(x => x.Id)
            .Include(x => x.LastName)
            .Include(x => x.UrlName)
            .Include(x => x.Club.Id);

        var playerModels = await _context.For<PlayerEntity>()
            .Find(x => true)
            .Project<PlayerEntity>(projection)
            .ToListAsync();

        var result = new List<PlayerEntity>();

        var actualPlayers = playerModels.Where(x => x.LastName != null && x.LastName.Length > 3)
            .OrderByDescending(x => x.LastName.Length);

        foreach (var playerModel in actualPlayers)
        {
            playerModel.LastName = playerModel.LastName?.ToLower();
            result.Add(playerModel);
        }

        return result.Map<List<PlayerModel>>();
    }

    private async Task<List<PlayerModel>> GetTopClubPlayerModels(string clubUrlName)
    {
        var filter = Builders<PlayerEntity>.Filter.Where(x => x.Club.UrlName == clubUrlName);

        var result = await _context.For<PlayerEntity>()
            .Find(filter)
            .SortByDescending(x => x.MentionsCount)
            .Limit(12)
            .ToListAsync();

        return result.Map<List<PlayerModel>>();
    }

    private async Task<List<PlayerModel>> GetTopGlobalPlayerModels()
    {
        var dateTime = DateTime.UtcNow.AddYears(-3);

        var filter = Builders<MentionEntity>.Filter
            .Where(x => x.Player.UrlName != null && x.Date > dateTime);

        var topPlayerModels = await _context.For<MentionEntity>()
            .Aggregate(new AggregateOptions { AllowDiskUse = true })
            .Match(filter)
            .SortByCount(x => x.Player.Id)
            .Limit(12)
            .ToListAsync();

        var playerLookup = topPlayerModels.ToLookup(x => x.Id, y => y.Count);

        var playerIds = topPlayerModels.Select(x => x.Id);

        var result = await _context.For<PlayerEntity>()
            .Find(x => playerIds.Contains(x.Id))
            .ToListAsync();

        var preResult = result
            .OrderByDescending(x => playerLookup[x.Id].FirstOrDefault())
            .ToList();

        return preResult.Map<List<PlayerModel>>();
    }

    public async Task<List<PlayerModel>> GetAllUrls()
    {
        var projection = Builders<PlayerEntity>
            .Projection
            .Include(x => x.UrlName);

        var result = await _context.For<PlayerEntity>()
            .Find(x => true)
            .Project<PlayerEntity>(projection)
            .ToListAsync();

        return result.Map<List<PlayerModel>>();
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
}