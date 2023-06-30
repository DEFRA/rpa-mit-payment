using EST.MIT.Payment.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using EST.MIT.Payment.Interfaces;

namespace EST.MIT.Payment.Services;

public class StrategicPaymentTransactionJsonGenerator : IStrategicPaymentTransactionJsonGenerator
{
    public string Generate(StrategicPaymentTransaction strategicPaymentTransaction)
    {
        var jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        return JsonConvert.SerializeObject(strategicPaymentTransaction, jsonSettings);
    }
}
