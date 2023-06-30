using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using EST.MIT.Payment.Models;

namespace EST.MIT.Payment.Function.Functions;

public static class StrategicPayments
{
    [FunctionName("ExecuteStrategicPayments")]
    public static async Task ExecuteStrategicPayments([ActivityTrigger] InvoiceScheme invoiceScheme, ILogger log)
    {
        log.LogInformation("Executing Strategic Payments...");
        // Add logic for Strategic Payments

        await Task.Delay(1); //TODO: remove when async code is added to the function
    }
}