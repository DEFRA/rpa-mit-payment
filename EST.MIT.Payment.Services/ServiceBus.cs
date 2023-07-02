using Azure.Messaging.ServiceBus;
using EST.MIT.Payment.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace EST.MIT.Payment.Services
{
    public class ServiceBus : IServiceBus
    {
        private readonly IConfiguration _configuration;

        public ServiceBus(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendServiceBus(string serviceBus)
        {
            var connectionString = _configuration["ConnectionString"];
            var queueName = _configuration["QueueName"];

            await using var client = new ServiceBusClient(connectionString);

            ServiceBusSender sender = client.CreateSender(queueName);

            ServiceBusMessage message = new(Encoding.UTF8.GetBytes(serviceBus));

            await sender.SendMessageAsync(message,default);

            await sender.DisposeAsync();
            await client.DisposeAsync();
        }
    }
}
