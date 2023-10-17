using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using Azure.Storage.Queues;
using Azure.Identity;
using EST.MIT.Payment.Interfaces;
using EST.MIT.Payment.Services;
using Azure.Messaging.ServiceBus;

namespace EST.MIT.Payment.Function.Services
{
    /// <summary>
    /// Register service-tier services.
    /// </summary>
    public static class ServicesConfiguration
    {
        /// <summary>
        /// Method to register service-tier services.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddQueueAndServiceBusServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEventQueueService>(_ =>
            {
                var storageAccountCredential = configuration.GetSection("QueueConnectionString:Credential").Value;
                var queueName = configuration.GetSection("EventQueueName").Value;
                if (IsManagedIdentity(storageAccountCredential))
                {
                    var queueServiceUri = configuration.GetSection("QueueConnectionString:QueueServiceUri").Value;
                    var queueUrl = new Uri($"{queueServiceUri}{queueName}");
                    return new EventQueueService(new QueueClient(queueUrl, new DefaultAzureCredential()));
                }
                else
                {
                    return new EventQueueService(new QueueClient(configuration.GetSection("QueueConnectionString").Value, queueName));
                }
            });

            services.AddSingleton<IServiceBus>(_ =>
            {
                var storageAccountCredential = configuration.GetSection("ServiceBusConnectionString:Credential").Value;
                var serviceBusQueueName = configuration.GetSection("ServiceBusQueueName").Value;
                if (IsManagedIdentity(storageAccountCredential))
                {
                    var serviceBusNamespace = configuration.GetSection("ServiceBusConnectionString:FullyQualifiedNamespace").Value;
                    var serviceBusClient = new ServiceBusClient(serviceBusNamespace, new DefaultAzureCredential());
                    return new ServiceBus(serviceBusQueueName, serviceBusClient);
                }
                else
                {
                    var serviceBusConnString = configuration.GetSection("ServiceBusConnectionString").Value;
                    var serviceBusClient = new ServiceBusClient(serviceBusConnString);
                    return new ServiceBus(serviceBusQueueName, serviceBusClient);
                }
            });
        }
        private static bool IsManagedIdentity(string credentialName)
        {
            return credentialName != null && credentialName.ToLower() == "managedidentity";
        }
    }
}
    