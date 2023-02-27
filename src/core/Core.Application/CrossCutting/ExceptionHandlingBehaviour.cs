using Core.Application.Common;
using Core.Application.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Core.Application.CrossCutting
{
    public class ExceptionHandlingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
        where TResponse : BaseResult
    {
        private readonly ILogger _logger;

        public ExceptionHandlingBehaviour(ILogger<ExceptionHandlingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response;

            try
            {
                response = await next();
            }
            catch (BusinessException businessException)
            {
                response = (TResponse)BaseResult.Failure(businessException.ErrorCode, businessException.Message);
            }
            catch(ValidationException validationException)
            {
                var errorDetail = validationException.Errors.FirstOrDefault();
                return (TResponse)BaseResult.Failure("7777", errorDetail?.ErrorMessage);
            }
            catch(Exception ex)
            {
                return (TResponse)BaseResult.Failure("9999", "Technical Exception");
            }

            _logger.LogError(JsonConvert.SerializeObject(response));

            return response;
        }
    }
}
