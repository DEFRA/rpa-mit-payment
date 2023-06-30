using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using EST.MIT.Payment.Models;

namespace EST.MIT.Payment.Function.Functions;

public static class TransformationLayer
{
    [FunctionName("ExecuteTransformationLayer")]
    public static async Task ExecuteTransformationLayer([ActivityTrigger] InvoiceScheme invoiceScheme, ILogger log)
    {
        log.LogInformation("Executing Transformation Layer...");
        // Add logic for Transformation Layer

        await Task.Delay(1); //TODO: remove when async code is added to the function
    }
}