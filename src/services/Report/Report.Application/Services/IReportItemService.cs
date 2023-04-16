using Report.Domain.Entities;

namespace Report.Application.Services
{
    public interface IReportItemService
    {
        Task<List<ReportItem>> Get();
        Task<ReportItem> Get(string id);
        Task<List<ReportItem>> GetByCorrelationId(Guid correlationId);
        Task<ReportItem> Create(ReportItem todoItem);
        Task Update(ReportItem todoItem);
        Task Remove(string id);
    }
}
