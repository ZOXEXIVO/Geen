namespace Geen.Core.Interfaces.Common
{
    public interface IQuery<TResult>
    {
    }

    public interface IQueryDispatcher
    {
        TResult Execute<TResult>(IQuery<TResult> query);
    }

    public interface IQueryHandler<in TQuery, out TResult>
        where TQuery : IQuery<TResult>
    {
        TResult Execute(TQuery query);
    }
}
