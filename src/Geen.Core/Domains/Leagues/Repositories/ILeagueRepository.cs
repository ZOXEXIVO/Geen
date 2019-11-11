using System.Collections.Generic;
using System.Threading.Tasks;

namespace Geen.Core.Domains.Leagues.Repositories
{
    public interface ILeagueRepository
    {
        Task<LeagueModel> GetById(int id);
        Task<LeagueModel> GetByUrlName(string urlName);

        Task<List<LeagueModel>> GetAll();

        Task<long> GetNextId();

        Task Save(LeagueModel model);
    }
}
