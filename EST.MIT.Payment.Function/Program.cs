using System;
using System.IO;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Queues;
using EST.MIT.Payment.DataAccess;
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
        var eventQueueName = configuration.GetSection("EventQueueName").Value;
        var queueConnectionString = configuration.GetSection("QueueConnectionString").Value;

        var connectionString = configuration.GetSection("ConnectionString").Value;
        var queueName = configuration.GetSection("QueueName").Value;


        services.AddScoped<IPaymentAuditRepository, PaymentAuditRepository>();
        services.AddScoped<IPaymentAuditProvider, PaymentAuditProvider>();

        services.AddSingleton<IEventQueueService>(_ =>
        {
            var eventQueueClient = new QueueClient(queueConnectionString, eventQueueName);
            return new EventQueueService(eventQueueClient);
        });

        services.AddSingleton<IServiceBus>(_ =>
        {
            var serviceBusClient = new ServiceBusClient(connectionString);
            var serviceBusMessage = new ServiceBusMessage();
            return new ServiceBus(queueName, serviceBusClient, serviceBusMessage);
        });
    })
    .Build();

host.Run();