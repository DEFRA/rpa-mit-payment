using System.Diagnostics.CodeAnalysis;
using EST.MIT.Payment.Function;
using EST.MIT.Payment.Interfaces;
using EST.MIT.Payment.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using EST.MIT.Payment.DataAccess;
using Azure.Storage.Queues;

[assembly: FunctionsStartup(typeof(Startup))]
namespace EST.MIT.Payment.Function;

[ExcludeFromCodeCoverage]
public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient();
        builder.Services.AddScoped<ISchemeValidator, SchemeValidator>();
        builder.Services.AddScoped<IServiceBus, ServiceBus>();
        var eventQueueName = builder.GetContext().Configuration["EventQueueName"];
        var queueConnectionString = builder.GetContext().Configuration["QueueConnectionString"];
        builder.Services.AddScoped<IPaymentAuditRepository, PaymentAuditRepository>();
        builder.Services.AddScoped<IPaymentAuditProvider, PaymentAuditProvider>();

        builder.Services.AddSingleton<IEventQueueService>(_ =>
        {
            var eventQueueClient = new QueueClient(queueConnectionString, eventQueueName);
            return new EventQueueService(eventQueueClient);
        });
    }
}
