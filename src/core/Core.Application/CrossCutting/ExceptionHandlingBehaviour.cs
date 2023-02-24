using MediatR;

namespace Core.Application.CrossCutting
{
    public class ExceptionHandlingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch(Exception)
            {
                // TODO:
                return await next();
            }
        }
    }
}
