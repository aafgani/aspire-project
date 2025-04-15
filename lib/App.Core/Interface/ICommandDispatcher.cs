using App.Domain.Model;

namespace App.Domain.Interface
{
    public interface ICommandDispatcher
    {
        Task<Result> DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand;
    }
}
