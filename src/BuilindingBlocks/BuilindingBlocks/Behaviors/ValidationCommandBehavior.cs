using Cortex.Mediator.Commands;
using FluentValidation;

namespace BuildingBlocks.Behaviors;

public class ValidationCommandBehavior<TCommand, TResult>(IEnumerable<IValidator<TCommand>> validators) 
    : ICommandPipelineBehavior<TCommand, TResult>  
    where TCommand : ICommand<TResult>
    where TResult : notnull
{
    public async Task<TResult> Handle(TCommand command, CommandHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TCommand>(command);

        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}
