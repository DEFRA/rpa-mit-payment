using EST.MIT.Payment.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using EST.MIT.Payment.Interfaces;
using Microsoft.Extensions.Configuration;
using Azure.Messaging.ServiceBus;

namespace EST.MIT.Payment.Services;

public class StrategicPaymentTransactionJsonGenerator : IStrategicPaymentTransactionJsonGenerator
{
    private readonly IConfiguration _configuration;
    private readonly ServiceBusClient _serviceBusClient;
    private readonly ServiceBusMessage _serviceBusMessage;

    public StrategicPaymentTransactionJsonGenerator(IConfiguration configuration, ServiceBusClient servicebusclient, ServiceBusMessage serviceBusMessage)
    {
        _configuration = configuration;
        _serviceBusClient = servicebusclient;
        _serviceBusMessage = serviceBusMessage;
    }

    public string Generate(StrategicPaymentTransaction strategicPaymentTransaction)
    {
        if (strategicPaymentTransaction.paymentInstruction.Error != null)
        {
            strategicPaymentTransaction.Accepted = false;

            return strategicPaymentTransaction.paymentInstruction.Error;
        }

        var jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        return JsonConvert.SerializeObject(strategicPaymentTransaction, jsonSettings);
    }
}
