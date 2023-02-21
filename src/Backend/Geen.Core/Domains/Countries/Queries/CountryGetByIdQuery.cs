using System.Threading.Tasks;
using Geen.Core.Domains.Countries.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Countries.Queries;

public record CountryGetByIdQuery : IQuery<Task<CountryModel>>
{
    public int Id { get; set; }
}

public class CountryGetByIdQueryHandler : IQueryHandler<CountryGetByIdQuery, Task<CountryModel>>
{
    private readonly ICountryRepository _countryRepository;

    public CountryGetByIdQueryHandler(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    public Task<CountryModel> Execute(CountryGetByIdQuery query)
    {
        return _countryRepository.GetById(query.Id);
    }
}