using MassTransit;
using Messages;
using Report.API.Consumers;
using Report.Application;
using Report.Infrastructure;

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
        cfg.AddConsumer<OrderPlacedEventConsumer>();
        cfg.AddConsumer<StockDecreasedEventConsumer>();
        cfg.AddConsumer<StockFailedEventConsumer>();
        cfg.AddConsumer<PaymentFailedEventConsumer>();
        cfg.AddConsumer<PaymentSucceededEventConsumer>();

        return cfg;
    };
    x.Endpoints = (context, cfg) =>
    {
        cfg.ReceiveEndpoint(string.Concat(RabbitMqConsts.OrderPlacedQueueName, "_", RabbitMqConsts.ReportApplicationName), e => {
            e.ConfigureConsumer<OrderPlacedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint(string.Concat(RabbitMqConsts.StockDecreasedQueueName, "_", RabbitMqConsts.ReportApplicationName), e => {
            e.ConfigureConsumer<StockDecreasedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint(string.Concat(RabbitMqConsts.StockFailedQueueName, "_", RabbitMqConsts.ReportApplicationName), e => {
            e.ConfigureConsumer<StockFailedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint(string.Concat(RabbitMqConsts.PaymentSucceededQueueName, "_", RabbitMqConsts.ReportApplicationName), e => {
            e.ConfigureConsumer<PaymentSucceededEventConsumer>(context);
        });

        cfg.ReceiveEndpoint(string.Concat(RabbitMqConsts.PaymentFailedQueueName, "_", RabbitMqConsts.ReportApplicationName), e => {
            e.ConfigureConsumer<PaymentFailedEventConsumer>(context);
        });
    };
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
