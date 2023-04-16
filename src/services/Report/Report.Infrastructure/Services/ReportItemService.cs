using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Report.Application.Options;
using Report.Application.Services;
using Report.Domain.Entities;

namespace Report.Infrastructure.Services
{
    public class ReportItemService : IReportItemService
    {
        private readonly MongoOptions _mongoOptions;
        private readonly IMongoCollection<ReportItem> _reportItems;
        public ReportItemService(IOptions<MongoOptions> mongoOptions,
            IMongoClient mongoClient)
        {
            _mongoOptions = mongoOptions.Value;

            var database = mongoClient.GetDatabase(_mongoOptions.DatabaseName);
            _reportItems = database.GetCollection<ReportItem>(_mongoOptions.CollectionName);
        }

        public async Task<ReportItem> Create(ReportItem todoItem)
        {
            await _reportItems.InsertOneAsync(todoItem);

            return todoItem;
        }

        public async Task<List<ReportItem>> Get()
        {
            return (await _reportItems.FindAsync(t => true)).ToList();
        }

        public async Task<ReportItem> Get(string id)
        {
            return (await _reportItems.FindAsync(x => x.Id == id)).FirstOrDefault();
        }

        public async Task<List<ReportItem>> GetByCorrelationId(Guid correlationId)
        {
            return (await _reportItems.FindAsync(x => x.CorrelationId == correlationId)).ToList();
        }

        public async Task Remove(string id)
        {
            await _reportItems.DeleteOneAsync(x => x.Id == id);
        }

        public async Task Update(ReportItem todoItem)
        {
            await _reportItems.ReplaceOneAsync(x => x.Id == todoItem.Id, todoItem);
        }
    }
}
