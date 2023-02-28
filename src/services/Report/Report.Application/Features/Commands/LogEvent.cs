using Core.Application.Common;
using MediatR;
using System.Text.Json;
namespace Report.Application.Features.Commands
{
    public static class LogEvent
    {
        public class Command : IRequest<BaseResult>
        {
            public Guid UserId { get; set; }
            public Guid CorrelationId { get; set; }
            public string Type { get; set; }
            public object Data { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, BaseResult>
        {
            public Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                Console.WriteLine($"CorrelationId: {request.CorrelationId} | Event: {JsonSerializer.Serialize(request)}");

                return Task.FromResult(BaseResult.Success());
            }
        }
    }
}
