namespace Geen.Core.Interfaces.Common;

public interface ICommand<TResult>
{
}

public interface ICommandDispatcher
{
    TResult Execute<TResult>(ICommand<TResult> command);
}

public interface ICommandDispatcher<in TCommand, out TResult>
    where TCommand : ICommand<TResult>
{
    TResult Execute(TCommand query);
}