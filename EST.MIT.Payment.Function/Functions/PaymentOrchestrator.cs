using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using EST.MIT.Payment.Models;
using EST.MIT.Payment.Interfaces;

namespace EST.MIT.Payment.Function.Functions;

public class PaymentOrchestrator
{
    private readonly ISchemeValidator _schemeValidator;

    public PaymentOrchestrator(ISchemeValidator schemeValidator)
    {
        _schemeValidator = schemeValidator;
    }

    [FunctionName("PaymentOrchestrator")]
    public async Task RunOrchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context,
        ILogger log)
    {
        var invoiceScheme = context.GetInput<InvoiceScheme>();

        var schemeType = invoiceScheme.SchemeType;
        var schemeExists = _schemeValidator.ValueExists(schemeType);

        if (schemeExists)
        {
            await context.CallActivityAsync("ExecuteStrategicPayments", invoiceScheme);
            log.LogInformation("Executing Strategic Payments...");
            await context.CallActivityAsync("ExecuteServiceBusForSPS", invoiceScheme);
        }
        else
        {
            await context.CallActivityAsync("ExecuteTransformationLayer", invoiceScheme);
            log.LogInformation("Executing Transformation Layer...");
        }
    }
}