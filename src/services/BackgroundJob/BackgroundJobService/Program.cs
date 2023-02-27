using BackgroundJobService;
using BackgroundJobService.OutboxWorkers;
using Core.Application.Services;
using Core.Infrastructure.Dependencies;
using Core.Infrastructure.Services;
using Core.Infrastructure.Services.Interfaces;

IConfiguration Configuration = null;

IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, cfg) =>
        {

            var env = hostingContext.HostingEnvironment;
            Configuration = new ConfigurationBuilder()
                .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .Build();
        })
    .ConfigureServices(services =>
    {
        services.AddSingleton<IDbConnectionFactory>(x => new PostgreDbConnectionFactory("Order", Configuration.GetConnectionString("OrderDatabase")));
        //services.AddSingleton<IDbConnectionFactory>(x => new PostgreDbConnectionFactory("Stock", Configuration.GetConnectionString("StockDatabase")));

        services.AddMessageBus(new Core.Infrastructure.DependencyModels.MessageBusOptions {
            Host = Configuration["MessageBroker:Host"],
            UserName = Configuration["MessageBroker:UserName"],
            Password = Configuration["MessageBroker:Password"],
            Consumers = (cfg) =>
            {
                return cfg;
            },
            Endpoints = (context, config) =>
            {

            },
            IsProducer = true
        });

        services.AddHostedService<OrderOutboxWorker>();

    })
    .Build();

await host.RunAsync();
