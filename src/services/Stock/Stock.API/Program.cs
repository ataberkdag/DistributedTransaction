using MassTransit;
using Messages;
using Stock.API.Consumers;
using Stock.Application;
using Stock.Infrastructure;
using Stock.Infrastructure.Services;
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
        cfg.AddConsumer<OrderPlacedConsumer>();

        return cfg;
    };
    x.Endpoints = (context, cfg) =>
    {
        cfg.ReceiveEndpoint(string.Concat(RabbitMqConsts.OrderPlacedQueueName, "_", RabbitMqConsts.StockApplicationName), e => {
            e.ConfigureConsumer<OrderPlacedConsumer>(context);
        });
    };
    x.IntegrationEventBuilderType = typeof(StockIntegrationEventBuilder);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

var app = builder.Build();

// Configure the HTTP request pipeline.
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
