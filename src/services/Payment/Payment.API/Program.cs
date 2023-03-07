using MassTransit;
using Messages;
using Payment.API.Consumers;
using Payment.Application;
using Payment.Infrastructure;
using Payment.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.AddCustomLogging(builder.Configuration);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration, x => {
    x.Host = builder.Configuration["MessageBroker:Host"];
    x.UserName = builder.Configuration["MessageBroker:UserName"];
    x.Password = builder.Configuration["MessageBroker:Password"];
    x.Consumers = (cfg) =>
    {
        cfg.AddConsumer<StockDecreasedConsumer>();

        return cfg;
    };
    x.Endpoints = (context, cfg) =>
    {
        cfg.ReceiveEndpoint(string.Concat(RabbitMqConsts.StockDecreasedQueueName, "_", RabbitMqConsts.PaymentApplicationName), e => {
            e.ConfigureConsumer<StockDecreasedConsumer>(context);
        });
    };
    x.IntegrationEventBuilderType = typeof(PaymentIntegrationEventBuilder);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
