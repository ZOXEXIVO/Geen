using System.Collections.Generic;
using System.Threading.Tasks;

namespace Geen.Core.Domains.Players.Repositories
{
    public interface IPlayerRepository
    {
        Task<PlayerModel> GetById(int id);
        Task<PlayerModel> GetByUrlName(string urlName);
        
        Task<PlayerModel> GetClubCoach(string clubUrlName);

        Task<List<PlayerModel>> GetList(string query, string clubUrlName, int page);
        Task<List<PlayerModel>> GetCached();
        
        Task<List<string>> GetUrls();
        
        Task<List<PlayerModel>> GetByClubUrlName(string clubUrlName);
        Task<List<int>> GetIdsByClubAndPosition(string clubUrlName, int position);

        Task<List<PlayerModel>> GetByIds(List<int> ids);
        Task<List<PlayerModel>> GetTopPlayers(string clubUrlName);
        Task<List<PlayerModel>> GetRelatedPlayers(string urlName);
        
        Task<PlayerModel> GetRandom();

        Task<(PlayerModel Left, PlayerModel Right)> GetForVotes(int position);
        
        Task<List<PlayerModel>> GetAll();

        Task<long> GetNextId();

        Task IncrementMentionsCount(int id);

        Task Save(PlayerModel model);
        
        Task<List<PlayerModel>> Search(string query, int count);
    }
}
