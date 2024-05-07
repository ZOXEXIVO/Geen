using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Replies;
using Geen.Core.Domains.Replies.Queries;
using Geen.Core.Domains.Replies.Repositories;
using Geen.Data.Entities;
using Geen.Data.Extensions;
using Geen.Data.Storages.Mongo;
using Geen.Data.Utils;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Geen.Data.Repositories;

public class ReplyRepository(MongoContext context) : IReplyRepository
{
    public async Task<ReplyModel> GetById(string id)
    {
        var result = await context.For<ReplyEntity>()
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync();

        return result.Map<ReplyModel>();
    }

    public async Task<List<ReplyModel>> GetList(GetReplyListQuery query)
    {
        var filter = Builders<ReplyEntity>.Filter
            .Where(x => x.MentionId == query.MentionId && x.IsApproved);

        var result = await context.For<ReplyEntity>()
            .Find(filter)
            .SortBy(x => x.Date)
            .ToPagedAsync(query.Page);

        return result.Map<List<ReplyModel>>();
    }

    public async Task<List<ReplyModel>> GetLatestList(GetReplyLatestQuery query)
    {
        var mentionsQuery = context.For<MentionEntity>().AsQueryable();
        var replyQuery = context.For<ReplyEntity>().AsQueryable();

        var resultQuery = from reply in replyQuery
            join mention in mentionsQuery on reply.MentionId equals mention.Id
            where mention.Club.Id == query.ClubId
                  || mention.Player.Id == query.PlayerId
            select reply;

        var result = await resultQuery.Take(query.Count).ToListAsync();

        return result.Map<List<ReplyModel>>();
    }

    public async Task<List<ReplyModel>> GetUnapprovedList(GetReplyUnapprovedListQuery query)
    {
        var builder = Builders<ReplyEntity>.Filter;

        var filter = builder.Empty;

        if (query.IsApproved.HasValue) filter &= builder.Where(x => x.IsApproved == query.IsApproved.Value);

        if (query.MentionId.HasValue) filter &= builder.Where(x => x.MentionId == query.MentionId);

        var result = await context.For<ReplyEntity>()
            .Find(filter)
            .SortByDescending(x => x.Date)
            .ToPagedAsync(query.Page);

        return result.Map<List<ReplyModel>>();
    }

    public async Task Approve(string id)
    {
        var update = Builders<ReplyEntity>.Update
            .Set(x => x.IsApproved, true);

        var result = await context.For<ReplyEntity>()
            .FindOneAndUpdateAsync(x => x.Id == id, update);

        await IncRepliesCount(result.MentionId, 1);
    }

    public async Task Disapprove(string id)
    {
        var update = Builders<ReplyEntity>.Update
            .Set(x => x.IsApproved, false);

        var result = await context.For<ReplyEntity>()
            .FindOneAndUpdateAsync(x => x.Id == id, update);

        await IncRepliesCount(result.MentionId, -1);
    }

    public async Task UpdateText(string id, string text)
    {
        var update = Builders<ReplyEntity>.Update
            .Set(x => x.Text, text);

        var result = await context.For<ReplyEntity>()
            .FindOneAndUpdateAsync(x => x.Id == id, update);

        var updateMention = Builders<MentionEntity>.Update
            .Set(x => x.TitleChangeDate, DateTime.UtcNow);

        await context.For<MentionEntity>()
            .UpdateOneAsync(x => x.Id == result.MentionId, updateMention);
    }

    public Task Save(ReplyModel model)
    {
        var entity = model.Map<ReplyEntity>();

        return context.For<ReplyEntity>().InsertOneAsync(entity);
    }

    public Task Delete(string id)
    {
        return context.For<ReplyEntity>()
            .DeleteOneAsync(x => x.Id == id);
    }

    public async Task<List<ReplyModel>> GetAll(int count)
    {
        var replies = await context.For<ReplyEntity>()
            .Aggregate()
            .AppendStage<ReplyEntity>("{ $sample: { size: " + count + "} }")
            .Project(x => new { x.Id })
            .ToListAsync();

        return replies.Map<List<ReplyModel>>();
    }

    public Task UpdateUser(string id, string userName)
    {
        var update = Builders<ReplyEntity>.Update
            .Set(x => x.User.IsAnonymous, false)
            .Set(x => x.User.Name, userName);

        return context.For<ReplyEntity>()
            .UpdateOneAsync(x => x.Id == id, update);
    }

    private Task IncRepliesCount(long mentionId, int value)
    {
        var mentionUpdate = Builders<MentionEntity>.Update
            .Inc(x => x.RepliesCount, value);

        return context.For<MentionEntity>()
            .UpdateOneAsync(x => x.Id == mentionId, mentionUpdate);
    }
}