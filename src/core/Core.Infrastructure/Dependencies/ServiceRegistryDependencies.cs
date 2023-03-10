using Consul;
using Core.Infrastructure.DependencyModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.Infrastructure.Dependencies
{
    public interface IServiceRegistryService : IHostedService
    {

    }

    public class ConsulRegisterService : IServiceRegistryService
    {
        private readonly IConsulClient _consulClient;
        private readonly ServiceRegistryOptions _options;
        private readonly ILogger _logger;
        public ConsulRegisterService(IConsulClient consulClient, 
            IOptions<ServiceRegistryOptions> options,
            ILogger<ConsulRegisterService> logger)
        {
            _consulClient = consulClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var serviceAddressUri = new Uri(this._options.ServiceAddress);
            var serviceRegistration = new AgentServiceRegistration()
            {
                Address = serviceAddressUri.Host,
                Port = serviceAddressUri.Port,
                Name = _options.ServiceName,
                ID = _options.ServiceId,
                Tags = new[] {_options.ServiceName}
            };

            await _consulClient.Agent.ServiceDeregister(_options.ServiceId, cancellationToken);
            await _consulClient.Agent.ServiceRegister(serviceRegistration, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _consulClient.Agent.ServiceDeregister(_options.ServiceId, cancellationToken);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Consul De-Register Error: {ex}");
            }
        }
    }
}
