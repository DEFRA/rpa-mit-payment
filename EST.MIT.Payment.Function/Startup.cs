using System.Diagnostics.CodeAnalysis;
using EST.MIT.Payment.Function;
using EST.MIT.Payment.Interfaces;
using EST.MIT.Payment.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using EST.MIT.Payment.DataAccess;
using Azure.Storage.Queues;
using Azure.Messaging.ServiceBus;

[assembly: FunctionsStartup(typeof(Startup))]
namespace EST.MIT.Payment.Function;

[ExcludeFromCodeCoverage]
public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient();
        builder.Services.AddScoped<ISchemeValidator, SchemeValidator>();
        var eventQueueName = builder.GetContext().Configuration["EventQueueName"];
        var queueConnectionString = builder.GetContext().Configuration["QueueConnectionString"];

        var connectionString = builder.GetContext().Configuration["ConnectionString"];
        var queueName = builder.GetContext().Configuration["QueueName"];


        builder.Services.AddScoped<IPaymentAuditRepository, PaymentAuditRepository>();
        builder.Services.AddScoped<IPaymentAuditProvider, PaymentAuditProvider>();

        builder.Services.AddSingleton<IEventQueueService>(_ =>
        {
            var eventQueueClient = new QueueClient(queueConnectionString, eventQueueName);
            return new EventQueueService(eventQueueClient);
        });

        builder.Services.AddSingleton<IServiceBus>(_ =>
        {
            var serviceBusClient = new ServiceBusClient(connectionString);
            var serviceBusMessage = new ServiceBusMessage();
            return new ServiceBus(queueName, serviceBusClient, serviceBusMessage);
        });
    }
}
