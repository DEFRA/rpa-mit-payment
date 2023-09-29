using System;
using EST.MIT.Payment.Function.Util;
using EST.MIT.Payment.Function.Validation;
using System.Threading.Tasks;
using EST.MIT.Payment.Interfaces;
using EST.MIT.Payment.Models;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using EST.MIT.Payment.Services;

namespace EST.MIT.Payment.Function.Functions
{
	public class CreatePayment
	{
        private readonly IEventQueueService _eventQueueService;
        private readonly IServiceBus _serviceBus;
        private readonly ISchemeValidator _schemeValidator;

        public CreatePayment(IEventQueueService eventQueueService, IServiceBus serviceBus, ISchemeValidator schemeValidator)
        {
            _eventQueueService = eventQueueService;
            _schemeValidator = schemeValidator;
            _serviceBus = serviceBus;
        }

        [Function("CreatePayment")]
        public async Task Run(
            [QueueTrigger("payment", Connection = "QueueConnectionString")] string paymentRequestMsg,
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

            var schemeType = invoiceScheme.SchemeType;
            var schemeExists = _schemeValidator.ValueExists(schemeType);

            if (schemeExists)
            {
                //await context.CallActivityAsync("ExecuteStrategicPayments", invoiceScheme);
                log.LogInformation("Executing Strategic Payments...");
                // Add logic for Strategic Payments

                await Task.Delay(1); //TODO: remove when async code is added to the function

                //await context.CallActivityAsync("ExecuteServiceBusForSPS", invoiceScheme);
                log.LogInformation("Executing Service Bus For Strategic Payments...");

                string message = JsonConvert.SerializeObject(invoiceScheme);

                await _serviceBus.SendServiceBus(message);
            }
            else
            {
                //await context.CallActivityAsync("ExecuteTransformationLayer", invoiceScheme);
                log.LogInformation("Executing Transformation Layer...");
                // Add logic for Transformation Layer

                await Task.Delay(1); //TODO: remove when async code is added to the function

            }
        }
    }
}

