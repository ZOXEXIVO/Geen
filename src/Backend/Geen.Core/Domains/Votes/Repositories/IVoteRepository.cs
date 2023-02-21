using System.Threading.Tasks;

namespace Geen.Core.Domains.Votes.Repositories;

public interface IVoteRepository
{
    Task Create(VoteModel model);
}