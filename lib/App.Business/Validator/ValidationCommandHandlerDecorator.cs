using App.Domain.Interface;
using App.Domain.Model;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace App.Business.Validator
{
    public class ValidationCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _inner;
        private readonly IEnumerable<IValidator<TCommand>>? _validators;

        public ValidationCommandHandlerDecorator(
            ICommandHandler<TCommand> inner, 
            IServiceProvider serviceProvider)
        {
            _inner = inner;
            _validators = serviceProvider.GetService<IEnumerable<IValidator<TCommand>>>();
        }

        public async Task<Result> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
        {
            if (_validators is not null)
            {
                var context = new ValidationContext<TCommand>(command);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken))
                    );

                var failures = validationResults
                    .SelectMany(result => result.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Any())
                {
                    var errorMessages = failures.Select(f => f.ErrorMessage);
                    return Result.Failure(errorMessages.ToArray());
                }
            }

            return await _inner.HandleAsync(command, cancellationToken);
        }
    }
}
