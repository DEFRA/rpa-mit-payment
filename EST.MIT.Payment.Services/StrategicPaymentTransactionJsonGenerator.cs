using EST.MIT.Payment.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using EST.MIT.Payment.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EST.MIT.Payment.Services;

public class StrategicPaymentTransactionJsonGenerator : IStrategicPaymentTransactionJsonGenerator
{
    private readonly IConfiguration _configuration;

    public StrategicPaymentTransactionJsonGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Generate(StrategicPaymentTransaction strategicPaymentTransaction)
    {
        var jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        return JsonConvert.SerializeObject(strategicPaymentTransaction, jsonSettings);
    }

    public async Task Send(StrategicPaymentTransaction strategicPayment)
    {
        var payment = Generate(strategicPayment);

        var transaction = new ServiceBus(_configuration);

        await transaction.SendServiceBus(payment);
    }
}
