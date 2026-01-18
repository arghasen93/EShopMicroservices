using Cortex.Mediator.Commands;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LoggingCommandBehavior<TCommand, TResult>(ILogger<LoggingCommandBehavior<TCommand, TResult>> logger) : ICommandPipelineBehavior<TCommand, TResult>
    where TCommand : ICommand<TResult>
    where TResult : notnull
{
    public async Task<TResult> Handle(TCommand command, CommandHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handle request={Request} response={Response}", typeof(TCommand).Name, typeof(TResult).Name);
        var stopwatch = Stopwatch.StartNew();
        stopwatch.Start();
        var response = await next();
        stopwatch.Stop();
        var timeTaken = stopwatch.Elapsed;
        if (timeTaken.TotalMilliseconds > 3)
        {
            logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken}",
                typeof(TCommand).Name, timeTaken);
        }
        logger.LogInformation("[END] Handle request={Request} response={Response}", typeof(TCommand).Name, typeof(TResult).Name);
        return response;
    }
}
