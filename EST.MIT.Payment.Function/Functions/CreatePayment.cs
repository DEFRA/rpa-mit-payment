using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using EST.MIT.Payment.Models;
using EST.MIT.Payment.Interfaces;
using EST.MIT.Payment.Function.Util;
using EST.MIT.Payment.Function.Validation;

namespace EST.MIT.Payment.Function.Functions;
public class CreatePayment
{
    private readonly IEventQueueService _eventQueueService;
    public CreatePayment(IEventQueueService eventQueueService)
    {
        _eventQueueService = eventQueueService;
    }

    [FunctionName("CreatePayment")]
    public async Task Run(
    [QueueTrigger("payment", Connection = "QueueConnectionString")] string paymentRequestMsg,
    [DurableClient] IDurableOrchestrationClient starter,
    IBinder blobBinder,
    ILogger log)
    {
        log.LogInformation($"C# Queue trigger function processed: {paymentRequestMsg}");

        InvoiceScheme invoiceScheme;

        if (paymentRequestMsg == null)
        {
            log.LogError("Payment request is null");
            await _eventQueueService.CreateMessage("failed", "paymentrequest", "Payment request is null", paymentRequestMsg);
            return;
        }

        var isValid = ValidateRequest.IsValid(paymentRequestMsg);

        if (!isValid)
        {
            log.LogError("Payment request is not valid");
            return;
        }

        try
        {
            invoiceScheme = JsonConvert.DeserializeObject<InvoiceScheme>(paymentRequestMsg);

            if (invoiceScheme.SchemeType == null)
            {
                await _eventQueueService.CreateMessage("failed", "paymentrequest", "payment request is Transformation Layer", paymentRequestMsg);
            }
        }
        catch (JsonException ex)
        {
            log.LogError(ex, "Error deserializing payment request");
            return;
        }

        log.LogInformation($"Payment request size: {MessageSize.GetMessageSize(invoiceScheme)}kb");

        await _eventQueueService.CreateMessage("sent", "paymentrequest", "payment request sent", paymentRequestMsg);

        var instanceId = await starter.StartNewAsync("PaymentOrchestrator", invoiceScheme);

        log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
    }
}
