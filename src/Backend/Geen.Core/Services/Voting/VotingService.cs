//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Geen.Core.Interfaces.Repositories;

//namespace Geen.Core.Services.Voting
//{
//    public class VotingService : IService
//    {
//        private readonly IQueryDispatcher queryDispatcher;

//        private static readonly ThreadLocal<Random> Random = 
//            new ThreadLocal<Random>(() => new Random(Environment.TickCount));

//        public VotingService(IPlayerRepository playerRepository)
//        {
//            _playerRepository = playerRepository;
//        }

//        public async Task<PlayerVoteModel> GetClubVotingModel(string clubUrlName)
//        {
//            var onePositionPlayersIds = await _playerRepository.GetPlayerIdsByClubAndPosition(
//                     clubUrlName, GetRandomPlayerPosition()
//                 );

//            var players = await _playerRepository.GetPlayersByIds(
//                GetRandomNPlayersIds(onePositionPlayersIds, 2)
//            );

//            if (players.Count < 2)
//                return null;

//            return new PlayerVoteModel
//            {
//                PlayerLeft = players[0],
//                PlayerRight = players[1]
//            };
//        }

//        private List<int> GetRandomNPlayersIds(List<int> playerIds, int n)
//        {
//            return playerIds.OrderBy(x => Guid.NewGuid()).Take(n).ToList();
//        }

//        private int GetRandomPlayerPosition()
//        {
//            return Random.Value.Next(0, 4);
//        }
//    }
//}
