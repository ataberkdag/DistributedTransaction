using MassTransit;
using Messages;
using Payment.API.Consumers;
using Payment.Application;
using Payment.Infrastructure;
using Payment.Infrastructure.Services;
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
