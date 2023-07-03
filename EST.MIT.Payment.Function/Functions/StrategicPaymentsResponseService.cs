using EST.MIT.Payment.Interfaces;
using EST.MIT.Payment.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace EST.MIT.Payment.Function.Functions
{

    public class StrategicPaymentsResponseService
    {
        private readonly IServiceBus _serviceBus;

        public StrategicPaymentsResponseService(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        [FunctionName("ExecuteServiceBusForSPS")]
        public async Task ExecuteServiceBusForSPS([ActivityTrigger] InvoiceScheme invoiceScheme, ILogger log)
        {
            log.LogInformation("Executing Service Bus For Strategic Payments...");

            string message = JsonConvert.SerializeObject(invoiceScheme);

            await _serviceBus.SendServiceBus(message);
        }
    }
}
