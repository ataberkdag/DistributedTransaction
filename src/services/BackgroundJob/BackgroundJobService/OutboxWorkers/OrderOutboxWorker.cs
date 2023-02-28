using Core.Application.Services;
using Core.Domain.Entities;
using Core.Infrastructure.Services.Interfaces;
using Dapper;
using Newtonsoft.Json;

namespace BackgroundJobService.OutboxWorkers
{
    internal class OrderOutboxWorker : BackgroundService
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IMassTransitHandler _massTransitHandler;

        public OrderOutboxWorker(IDbConnectionFactory dbConnectionFactory,
            IMassTransitHandler massTransitHandler)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _massTransitHandler = massTransitHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                string sql = $@"         SELECT
                                          ""Id"",
                                          ""Type"",
                                          ""Data"",
                                          ""QueueName"",
                                          ""CreatedOn""
                                     FROM public.""OutboxMessages""
                                     ORDER BY ""Id""
                                     LIMIT 100 FOR UPDATE SKIP LOCKED; 
                                ";

                var connection = _dbConnectionFactory.GetOpenConnection();

                var messages = await connection.QueryAsync<OutboxMessage>(sql);

                var listOfIds = new List<long>();
                foreach (var outboxMessage in messages)
                {
                    try
                    {
                        var outboxMessageData = JsonConvert.DeserializeObject(outboxMessage.Data, Type.GetType(outboxMessage.Type));

                        if (!String.IsNullOrEmpty(outboxMessage.QueueName))
                        {
                            await this._massTransitHandler.Send(outboxMessage.QueueName, outboxMessageData);
                        }
                        else
                        {
                            await this._massTransitHandler.Publish(outboxMessageData);
                        }

                        listOfIds.Add(outboxMessage.Id);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex?.Message);
                    }
                }

                if (listOfIds.Count > 0)
                {
                    var transaction = connection.BeginTransaction();
                    await connection.ExecuteAsync($@"DELETE FROM public.""OutboxMessages"" WHERE ""Id"" IN ('{string.Join("','", listOfIds)}')");
                    transaction.Commit();
                }
            }
        }
    }
}
