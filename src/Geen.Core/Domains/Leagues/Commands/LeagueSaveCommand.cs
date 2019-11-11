using System.Threading.Tasks;
using Geen.Core.Domains.Leagues.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Leagues.Commands
{
    public class LeagueSaveCommand : ICommand<Task>
    {
        public LeagueModel Model { get; set; }
    }

    public class LeagueSaveCommandDispatcher : ICommandDispatcher<LeagueSaveCommand, Task>
    {
        private readonly ILeagueRepository _leagueRepository;

        public LeagueSaveCommandDispatcher(ILeagueRepository leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }

        public Task Execute(LeagueSaveCommand command)
        {
            return _leagueRepository.Save(command.Model);
        }
    }
}
