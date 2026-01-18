using Cortex.Mediator.Queries;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LoggingQueryBehavior<TQuery, TResult>(ILogger<LoggingQueryBehavior<TQuery, TResult>> logger)
    : IQueryPipelineBehavior<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    public async Task<TResult> Handle(TQuery query, QueryHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handle request={Request} response={Response}", typeof(TQuery).Name, typeof(TResult).Name);
        var stopwatch = Stopwatch.StartNew();
        stopwatch.Start();
        var response = await next();
        stopwatch.Stop();
        var timeTaken = stopwatch.Elapsed;
        if (timeTaken.TotalMilliseconds > 3)
        {
            logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken}",
                typeof(TQuery).Name, timeTaken);
        }
        logger.LogInformation("[END] Handle request={Request} response={Response}", typeof(TQuery).Name, typeof(TResult).Name);
        return response;
    }
}
