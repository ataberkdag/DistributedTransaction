using MassTransit;
using Messages;
using Order.API.Consumers;
using Order.Application;
using Order.Infrastructure;
using Order.Infrastructure.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddCustomLogging(builder.Configuration);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration, Assembly.GetExecutingAssembly(), AppContext.BaseDirectory, x => {
    x.Host = builder.Configuration["MessageBroker:Host"];
    x.UserName = builder.Configuration["MessageBroker:UserName"];
    x.Password = builder.Configuration["MessageBroker:Password"];
    x.Consumers = (cfg) =>
    {
        cfg.AddConsumer<StockFailedConsumer>();
        cfg.AddConsumer<PaymentFailedConsumer>();
        cfg.AddConsumer<PaymentSucceededConsumer>();

        return cfg;
    };
    x.Endpoints = (context, cfg) =>
    {
        cfg.ReceiveEndpoint(string.Concat(RabbitMqConsts.StockFailedQueueName, "_", RabbitMqConsts.OrderApplicationName), e => {
            e.ConfigureConsumer<StockFailedConsumer>(context);
        });

        cfg.ReceiveEndpoint(string.Concat(RabbitMqConsts.PaymentFailedQueueName, "_", RabbitMqConsts.OrderApplicationName), e => {
            e.ConfigureConsumer<PaymentFailedConsumer>(context);
        });

        cfg.ReceiveEndpoint(string.Concat(RabbitMqConsts.PaymentSucceededQueueName, "_", RabbitMqConsts.OrderApplicationName), e => {
            e.ConfigureConsumer<PaymentSucceededConsumer>(context);
        });
    };
    x.IntegrationEventBuilderType = typeof(OrderIntegrationEventBuilder);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseApplication();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
