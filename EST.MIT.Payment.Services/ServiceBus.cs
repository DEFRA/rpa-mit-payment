using Azure.Messaging.ServiceBus;
using EST.MIT.Payment.Interfaces;
using System.Text;

namespace EST.MIT.Payment.Services
{
    public class ServiceBus : IServiceBus
    {
        private ServiceBusSender _sender = default!;

        private ServiceBusMessage _message;

        private readonly string _queueName;

        private readonly ServiceBusClient _client;

        public ServiceBus(string queueName, ServiceBusClient client, ServiceBusMessage message)
        {
            _queueName = queueName;

            _client = client;

            _message = message;
        }

        public async Task SendServiceBus(string serviceBus)
        {
            try
            {
                _sender = _client.CreateSender(_queueName);

                _message = new(Encoding.UTF8.GetBytes(serviceBus));

                await _sender.SendMessageAsync(_message);
            }

            catch (ServiceBusException ex) when
            (ex.Reason == ServiceBusFailureReason.MessagingEntityNotFound)
            {
                return;
            }

            catch (ServiceBusException ex) when
            (ex.Reason == ServiceBusFailureReason.ServiceTimeout)
            {
                return;
            }

            catch (ServiceBusException ex) when
            (ex.Reason == ServiceBusFailureReason.MessageSizeExceeded)
            {
                return;
            }
        }
    }
}
