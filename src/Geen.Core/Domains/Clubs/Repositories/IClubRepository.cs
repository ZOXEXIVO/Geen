using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Geen.Core.Domains.Clubs.Repositories
{
    public interface IClubRepository
    {
        Task<ClubModel> GetById(int id);
        Task<ClubModel> GetByUrlName(string urlName);

        Task<List<ClubModel>> GetAll();
        Task<List<ClubModel>> GetAllUrls();
        Task<List<ClubModel>> GetCached();

        Task<long> GetNextId();
        Task<List<DateTime>> GetBirthdays(string urlName);

        Task Save(ClubModel model);
    }
}
