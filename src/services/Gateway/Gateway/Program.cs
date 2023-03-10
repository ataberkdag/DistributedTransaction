using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("ocelot.json", false, true);

builder.Services
    .AddOcelot(builder.Configuration)
    .AddConsul();

builder.Services.AddControllers();

var app = builder.Build();

app.UseOcelot()
    .Wait();

app.UseAuthorization();

app.MapControllers();

app.Run();
