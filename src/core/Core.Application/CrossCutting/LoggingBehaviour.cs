using MediatR;

namespace Core.Application.CrossCutting
{
    // TODO: PipelineBehavior
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            Console.WriteLine("Logging");

            return await next();
        }
    }
}
