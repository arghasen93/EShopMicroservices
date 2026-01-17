using FluentValidation;
using Cortex.Mediator.Queries;

namespace BuildingBlocks.Behaviors;

public class ValidationQueryBehavior<TQuery, TResult>(IEnumerable<IValidator<TQuery>> validators)
    : IQueryPipelineBehavior<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    public async Task<TResult> Handle(TQuery query, QueryHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TQuery>(query);

        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}
