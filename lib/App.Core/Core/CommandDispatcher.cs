using App.Domain.Interface;
using App.Domain.Model;
using Microsoft.Extensions.DependencyInjection;

namespace App.Domain.Core
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<Result> DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand
        {
            var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
            return handler.HandleAsync(command, cancellationToken);
        }
    }
}
