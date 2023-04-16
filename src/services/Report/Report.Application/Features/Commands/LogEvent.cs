using Core.Application.Common;
using MediatR;
using Report.Application.Services;
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
            private readonly IReportItemService _reportItemService;
            public CommandHandler(IReportItemService reportItemService)
            {
                _reportItemService = reportItemService;
            }

            public Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                _reportItemService.Create(new Domain.Entities.ReportItem { 
                    CorrelationId = request.CorrelationId,
                    Request = JsonSerializer.Serialize(request)
                });
                
                Console.WriteLine($"LogEvent | CorrelationId: {request.CorrelationId}");

                return Task.FromResult(BaseResult.Success());
            }
        }
    }
}
