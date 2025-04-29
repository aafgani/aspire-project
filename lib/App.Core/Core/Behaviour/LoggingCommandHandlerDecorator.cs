using App.Domain.Interface;
using App.Domain.Model;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace App.Domain.Core.Behaviour
{
    public class LoggingCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _inner;
        private readonly ILogger<LoggingCommandHandlerDecorator<TCommand>> _logger;

        public LoggingCommandHandlerDecorator(
        ICommandHandler<TCommand> inner,
        ILogger<LoggingCommandHandlerDecorator<TCommand>> logger)
        {
            _inner = inner;
            _logger = logger;
        }

        public async Task<Result> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
        {
            var commandName = typeof(TCommand).Name;
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("[Logging] Handling command: {CommandType}", typeof(TCommand).Name);
            
            try
            {
                var result = await _inner.HandleAsync(command, cancellationToken);
                stopwatch.Stop();

                if (result.IsSuccess) 
                {
                    _logger.LogInformation("Successfully handled {CommandName} in {ElapsedMs}ms", commandName, stopwatch.ElapsedMilliseconds);
                }
                else
                {
                    _logger.LogWarning("Command {CommandName} failed with validation/business errors in {ElapsedMs}ms: {@Errors}",
                        commandName, stopwatch.ElapsedMilliseconds, result.Errors);
                }

                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Unhandled exception while handling {CommandName} after {ElapsedMs}ms", commandName, stopwatch.ElapsedMilliseconds);
                return Result.Failure("Unexpected error occurred");
            }
        }
    }
}
