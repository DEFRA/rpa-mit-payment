using System;
using System.IO;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Queues;
using EST.MIT.Payment.Interfaces;
using EST.MIT.Payment.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureAppConfiguration(config => config
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables())
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        Console.WriteLine("Program.Startup.ConfigureServices() called");
        var serviceProvider = services.BuildServiceProvider();

        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        services.AddHttpClient();
        services.AddScoped<ISchemeValidator, SchemeValidator>();

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
    })
    .Build();

host.Run();

static bool IsManagedIdentity(string credentialName)
{
    return credentialName != null && credentialName.ToLower() == "managedidentity";
}