using Core.Application.Common;
using MediatR;
using Report.Application.Services;

namespace Report.Application.Features.Queries
{
    public static class GetEvent
    {
        public class Query : IRequest<BaseResult>
        {
            public Guid CorrelationId { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, BaseResult>
        {
            private readonly IReportItemService _reportItemService;

            public QueryHandler(IReportItemService reportItemService)
            {
                _reportItemService = reportItemService;
            }

            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var reportItemList = await _reportItemService.GetByCorrelationId(request.CorrelationId);

                List<ReportItemDto> reportItemDtoList = new List<ReportItemDto>();
                
                reportItemList?.ForEach(reportItem => {
                    reportItemDtoList.Add(new ReportItemDto
                    {
                        SerializedReport = reportItem.Request
                    });
                });

                return BaseResult<Response>.Success(new Response { 
                    Items = reportItemDtoList
                });
            }
        }

        public class Response
        {
            public List<ReportItemDto> Items { get; set; }
        }

        public class ReportItemDto
        {
            public string SerializedReport { get; set; }
        }
    }
}
