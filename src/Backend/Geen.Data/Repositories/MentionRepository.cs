using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geen.Core.Domains.Mentions;
using Geen.Core.Domains.Mentions.Commands;
using Geen.Core.Domains.Mentions.Models;
using Geen.Core.Domains.Mentions.Queries;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Domains.Players.Utils;
using Geen.Core.Models.Likes;
using Geen.Data.Entities;
using Geen.Data.Extensions;
using Geen.Data.Storages.Mongo;
using Geen.Data.Utils;
using Mapster;
using MongoDB.Driver;

namespace Geen.Data.Repositories
{
    public class MentionRepository : IMentionRepository
    {
        private readonly MongoContext _context;
        private readonly IdentityService _identityService;

        public MentionRepository(MongoContext context, IdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<MentionModel> GetById(long identity)
        {
            var result = await _context.For<MentionEntity>()
                .Find(x => x.Id == identity)
                .FirstOrDefaultAsync();

            return result.Map<MentionModel>();
        }

        public async Task<List<MentionModel>> GetMentionIdentities()
        {
            var projection = Builders<MentionEntity>.Projection
                .Include(x => x.Id)
                .Include(x => x.RepliesCount)
                .Include(x => x.Player.UrlName)
                .Include(x => x.Club.UrlName);

            var result = await _context.For<MentionEntity>()
                .Find(x => x.IsApproved)
                .Project<MentionEntity>(projection)
                .ToListAsync();

            return result.Map<List<MentionModel>>();
        }

        public async Task<List<MentionModel>> GetMentionTitleList(long? id, int page)
        {
            var filter = Builders<MentionEntity>.Filter.Where(x => x.IsApproved);

            if (id.HasValue)
                filter &= Builders<MentionEntity>.Filter.Where(x => x.Id == id);

            var project = Builders<MentionEntity>.Projection
                .Include(x => x.Id)
                .Include(x => x.Text)
                .Include(x => x.Title);

            var result = await _context.For<MentionEntity>()
                .Find(filter)
                .SortByDescending(x => x.Date)
                .Project<MentionEntity>(project)
                .ToPagedAsync(page);

            return result.Map<List<MentionModel>>();
        }

        public async Task<List<MentionModel>> GetList(GetMentionListQuery query)
        {
            var builder = Builders<MentionEntity>.Filter;

            var filter = FilterDefinition<MentionEntity>.Empty;

            if (query.IsApproved.HasValue)
                filter = filter & builder.Eq(x => x.IsApproved, query.IsApproved.Value);

            if (query.PlayerId.HasValue)
                filter = filter & builder.Where(x => x.Player.Id == query.PlayerId.Value);

            if (!string.IsNullOrWhiteSpace(query.ClubUrlName))
                filter = filter & builder.Where(x => x.Club.UrlName == query.ClubUrlName);

            if (!string.IsNullOrWhiteSpace(query.PlayerClubUrlName))
                filter = filter & builder.Where(x => x.Player.Club.UrlName == query.PlayerClubUrlName);

            if (!string.IsNullOrWhiteSpace(query.UserId))
                filter = filter & builder.Where(x => x.User.Id == query.UserId);

            if (query.ForPlayer.HasValue)
                filter = filter & builder.Exists(x => x.Player.Id, query.ForPlayer.Value);

            if (query.ForClub.HasValue)
            {
                filter = filter & builder.Exists(x => x.Club.Id, query.ForClub.Value)
                                & builder.Not(builder.Eq(x => x.Club.IsNational, true));
            }

            if (query.Latitude.HasValue && query.Longitude.HasValue)
            {
                filter = filter &
                         builder.Near(x => x.Location, query.Latitude.Value, query.Longitude.Value);
            }

            var result = await _context.For<MentionEntity>()
                .Find(filter)
                .SortByDescending(x => x.Date)
                .ToPagedAsync(query.Page);

            return result.Map<List<MentionModel>>();
        }
        
        public async Task<long> GetNextId()
        {
            var projection = Builders<MentionEntity>.Projection.Expression(x => (int?)x.Id);

            var lastId = await _context.For<MentionEntity>()
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


        public Task<LikeModel> GetLikeStatus(long id)
        {
            var projection = Builders<MentionEntity>.Projection
                .Include(x => x.Likes)
                .Include(x => x.Dislikes);

            return _context.For<MentionEntity>()
                .Find(x => x.Id == id)
                .Project<LikeModel>(projection)
                .FirstOrDefaultAsync();
        }

        public async Task Approve(long id)
        {
            var update = Builders<MentionEntity>.Update.Set(x => x.IsApproved, true);

            var result = await _context.For<MentionEntity>()
                .FindOneAndUpdateAsync(x => x.Id == id, update);

            if (result.Player != null)
            {
                await _context.For<PlayerEntity>()
                    .UpdateOneAsync(x => x.Id == result.Player.Id,
                        Builders<PlayerEntity>.Update.Inc(x => x.MentionsCount, 1));
            }
        }

        public async Task Disapprove(long id)
        {
            var update = Builders<MentionEntity>.Update.Set(x => x.IsApproved, false);

            var result = await _context.For<MentionEntity>()
                .FindOneAndUpdateAsync(x => x.Id == id, update);

            if (result.Player != null)
            {
                await _context.For<PlayerEntity>()
                    .UpdateOneAsync(x => x.Id == result.Player.Id,
                        Builders<PlayerEntity>.Update.Inc(x => x.MentionsCount, -1));
            }
        }

        public Task Like(long id, IncrementStatus status)
        {
            var updates = new List<UpdateDefinition<MentionEntity>>();

            if (status.Likes != 0)
                updates.Add(Builders<MentionEntity>.Update.Inc(x => x.Likes, status.Likes));

            if (status.Dislikes != 0)
                updates.Add(Builders<MentionEntity>.Update.Inc(x => x.Dislikes, status.Dislikes));
            
            if (updates.Count == 0)
                throw new InvalidOperationException("Empty update list");
            
            var updateDefinition = Builders<MentionEntity>.Update.Combine(updates);

            return _context.For<MentionEntity>().UpdateOneAsync(
                mention => mention.Id == id,
                updateDefinition
            );
        }

        public Task Dislike(long id, IncrementStatus status)
        {
            var updates = new List<UpdateDefinition<MentionEntity>>();

            if (status.Likes != 0)
                updates.Add(Builders<MentionEntity>.Update.Inc(x => x.Likes, status.Likes));

            if (status.Dislikes != 0)
                updates.Add(Builders<MentionEntity>.Update.Inc(x => x.Dislikes, status.Dislikes));

            if(updates.Count == 0)
                throw new InvalidOperationException("Empty update list");

            return _context.For<MentionEntity>().UpdateOneAsync(
                mention => mention.Id == id,
                Builders<MentionEntity>.Update.Combine(updates)
            );
        }

        public Task UpdateTitle(long id, string title)
        {
            var update = Builders<MentionEntity>.Update
                .Set(x => x.Title, title)
                .Set(x => x.TitleChangeDate, DateTime.UtcNow);

            return _context.For<MentionEntity>()
                .UpdateOneAsync(x => x.Id == id, update);
        }

        public Task UpdateText(long id, string text)
        {
            var update = Builders<MentionEntity>.Update
                .Set(x => x.Text, text)
                .Set(x => x.TitleChangeDate, DateTime.UtcNow);

            return _context.For<MentionEntity>()
                .UpdateOneAsync(x => x.Id == id, update);
        }

        public Task UpdateUser(long id, string userName)
        {
            var update = Builders<MentionEntity>.Update
                .Set(x => x.User.IsAnonymous, false)
                .Set(x => x.User.Name, userName);

            return _context.For<MentionEntity>()
                .UpdateOneAsync(x => x.Id == id, update);
        }

        public Task SetDefaultAvatar(long id)
        {
            var update = Builders<MentionEntity>.Update
                .Set(x => x.User.ProfileImage, null);

            return _context.For<MentionEntity>()
                .UpdateOneAsync(x => x.Id == id, update);
        }

        public async Task Save(MentionModel model)
        {
            if (model.Id == 0)
                model.Id = await _identityService.Next<MentionEntity>();

            var entity = model.Map<MentionEntity>();

            await _context.For<MentionEntity>()
                .ReplaceOneAsync(x => x.Id == entity.Id, entity,
                    new ReplaceOptions { IsUpsert = true });
        }

        public Task Delete(long id)
        {
            return _context.For<MentionEntity>()
                .DeleteOneAsync(x => x.Id == id);
        }

        public Task IncrementRepliesCount(long id)
        {
            var update = Builders<MentionEntity>.Update
                .Inc(x => x.RepliesCount, 1);

            return _context.For<MentionEntity>()
                .UpdateOneAsync(x => x.Id == id, update);
        }

        public Task DecrementRepliesCount(long id)
        {
            var update = Builders<MentionEntity>.Update
                .Inc(x => x.RepliesCount, -1);

            return _context.For<MentionEntity>()
                .UpdateOneAsync(x => x.Id == id, update);
        }

        #region Fresh

        public async Task<List<MentionModel>> GetFreshMentions(DateTime? dateStart)
        {
            if (!dateStart.HasValue)
                dateStart = DateTime.UtcNow.Date;

            var mentionProjection = Builders<MentionEntity>.Projection
                .Include(x => x.Id)
                .Include(x => x.Player.UrlName)
                .Include(x => x.RepliesCount)
                .Include(x => x.Club.UrlName);

            var result = await _context.For<MentionEntity>()
                .Find(x => x.Date >= dateStart || x.TitleChangeDate >= dateStart)
                .Project<MentionEntity>(mentionProjection)
                .ToListAsync();

            return result.Map<List<MentionModel>>();
        }

        public async Task<List<MentionModel>> GetFreshTitledMentions(DateTime? dateStart)
        {
            if (!dateStart.HasValue)
                dateStart = DateTime.UtcNow.Date;

            var mentionProjection = Builders<MentionEntity>.Projection
                .Include(x => x.Id)
                .Include(x => x.Player.UrlName)
                .Include(x => x.RepliesCount)
                .Include(x => x.Club.UrlName);

            var result = await _context.For<MentionEntity>()
                .Find(x => x.IsApproved && x.TitleChangeDate >= dateStart)
                .Project<MentionEntity>(mentionProjection)
                .ToListAsync();

            return result.Map<List<MentionModel>>();
        }

        public async Task<List<MentionModel>> GetFreshRepliedMentionIds(DateTime? dateStart)
        {
            if (!dateStart.HasValue)
                dateStart = DateTime.UtcNow.Date;

            var repliesMentionIds = await _context.For<ReplyEntity>()
                .Find(x => x.IsApproved && x.Date >= dateStart)
                .Project(x => x.MentionId)
                .ToListAsync();

            var mentionProjection = Builders<MentionEntity>.Projection
                .Include(x => x.Id)
                .Include(x => x.Player.UrlName)
                .Include(x => x.RepliesCount)
                .Include(x => x.Club.UrlName);

            var result = await _context.For<MentionEntity>()
                .Find(x => repliesMentionIds.Contains(x.Id))
                .Project<MentionEntity>(mentionProjection)
                .ToListAsync();

            return result.Map<List<MentionModel>>();
        }

        public long GetClubItemsCount(string clubUrlName)
        {
            return _context.For<MentionEntity>()
                .CountDocuments(x => x.Club.UrlName == clubUrlName);
        }

        public long GetPlayerItemsCount(string playerUrlName)
        {
            return _context.For<MentionEntity>()
                .CountDocuments(x => x.Player.UrlName == playerUrlName);
        }

        public Task RandomLikes(MentionRandomLikerCommand command)
        {
            return Task.WhenAll(
                ForMentions()
            );

            async Task ForMentions()
            {
                var minDate = DateTime.UtcNow.Date.AddDays(-command.DaysInteval);

                var mentions = await _context.For<MentionEntity>()
                    .Aggregate()
                    .Match(x => x.Date >= minDate)
                    .AppendStage<MentionEntity>("{ $sample: { size: " + command.Count + "} }")
                    .Project(x => new { x.Id })
                    .ToListAsync();

                var tasks = new List<Task>();

                var randomizer = Randomizer.RandomLocal;
                
                foreach (var mention in mentions)
                {
                    var likesInc = randomizer.Next(0, command.MaxLikes);
                    var dislikesInc = randomizer.Next(0, command.MaxDislikes);

                    var update = Builders<MentionEntity>.Update
                        .Inc(x => x.Likes, likesInc)
                        .Inc(x => x.Dislikes, dislikesInc);

                    var updateTask = _context.For<MentionEntity>().UpdateOneAsync(x => x.Id == mention.Id, update);

                    tasks.Add(updateTask);
                }

                await Task.WhenAll(tasks);
            }
        }

        public async Task<List<MentionModel>> GetAll(int count)
        {
            var mentions = await _context.For<MentionEntity>()
                .Aggregate()
                .AppendStage<MentionEntity>("{ $sample: { size: " + count + "} }")
                .Project(x => new { x.Id })
                .ToListAsync();

            return mentions.Map<List<MentionModel>>();
        }

        #endregion
    }
}
