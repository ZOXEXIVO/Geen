using System.Threading.Tasks;
using Geen.Core.Domains.Clubs.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Clubs.Commands
{
    public class ClubSaveCommand : ICommand<Task>
    {
        public ClubModel Model { get; set; }
    }

    public class ClubSaveCommandDispatcher : ICommandDispatcher<ClubSaveCommand, Task>
    {
        private readonly IClubRepository _clubRepository;

        public ClubSaveCommandDispatcher(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public Task Execute(ClubSaveCommand command)
        {
            return _clubRepository.Save(command.Model);
        }
    }
}
