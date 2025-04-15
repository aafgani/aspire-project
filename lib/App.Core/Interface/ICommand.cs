using App.Domain.Model;

namespace App.Domain.Interface
{
    public class ICommand
    {
    }
    public interface ICommandHandler<TCommand> 
        where TCommand : ICommand
    {
        Task<Result> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }
}
