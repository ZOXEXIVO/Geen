using System.Collections.Generic;
using System.Threading.Tasks;

namespace Geen.Core.Domains.Countries.Repositories;

public interface ICountryRepository
{
    Task<CountryModel> GetById(int id);
    Task<CountryModel> GetByUrlName(string urlName);

    Task<List<CountryModel>> GetAll();

    Task<long> GetNextId();

    Task Save(CountryModel model);
}