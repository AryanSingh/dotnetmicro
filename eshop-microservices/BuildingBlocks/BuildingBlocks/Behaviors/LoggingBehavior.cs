using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger): IPipelineBehavior<TRequest, TResponse>
where TRequest: notnull, IRequest<TResponse>
where TResponse: notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {RequestType} with content: {@Request}", typeof(TRequest).Name, request);
        var timer = new Stopwatch();
        timer.Start();
        var response = await next();
        timer.Stop();
        var timeTaken = timer.Elapsed;
        logger.LogInformation("Handled {RequestType} in {TimeTaken} with response: {@Response}", typeof(TRequest).Name, timeTaken, response);
        return response;
    }
}