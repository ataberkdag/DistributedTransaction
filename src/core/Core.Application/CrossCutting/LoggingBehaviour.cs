using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Application.CrossCutting
{
    // TODO: PipelineBehavior
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest,TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Logger Working!");

            return await next();
        }
    }
}
