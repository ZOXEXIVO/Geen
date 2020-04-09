using System.Threading.Tasks;
using Geen.Core.Domains.Countries.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Countries.Commands
{
    public class CountrySaveCommand : ICommand<Task>
    {
        public CountryModel Model { get; set; }
    }

    public class CountrySaveCommandDispatcher : ICommandDispatcher<CountrySaveCommand, Task>
    {
        private readonly ICountryRepository _countryRepository;

        public CountrySaveCommandDispatcher(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public Task Execute(CountrySaveCommand command)
        {
            return _countryRepository.Save(command.Model);
        }
    }
}
