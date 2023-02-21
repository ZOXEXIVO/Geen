// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Geen.Core.Entities;
// using Geen.Data.Models;
// using Geen.Data.Mongo;
// using Geen.Data.Mongo.ConnectionProvider.Interfaces;
// using Geen.Data.Services.Mentions.Factories;
// using Geen.Data.Services.Mentions.Filters;
// using Geen.Data.Services.Mentions.Queries;
// using Geen.Data.Services.Players;
// using System.Linq.Expressions;
// using System.Linq;
// using Geen.Data.Utils;
// using MongoDB.Driver;

// namespace Geen.Data.Services.Mentions
// {
//     public class VotingService : IService
//     {
//         private readonly GeenContext _context;


//         public VotingService(GeenContext context, PlayerService playerService)
//         {
//             _context = context;
//             _playerService = playerService;
//         }

//         public async Task<PlayerVoteModel> GetClubVoteModel(int clubId)
//         {

//         }


//         public async Task<PlayerVoteModel> GetGlobalVoteModel()
//         {
//             var result = new PlayerVoteModel();

//             return result;
//         }
//     }
// }

