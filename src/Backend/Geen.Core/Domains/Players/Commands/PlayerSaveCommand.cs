using System.Threading.Tasks;
using Geen.Core.Domains.Players.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Players.Commands
{
    public class PlayerSaveCommand : ICommand<Task>
    {
        public PlayerModel Model { get; set; }
    }

    public class PlayerSaveCommandDispatcher : ICommandDispatcher<PlayerSaveCommand, Task>
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerSaveCommandDispatcher(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }
        
        public Task Execute(PlayerSaveCommand command)
        {
            return _playerRepository.Save(command.Model);
        }
    }
}
