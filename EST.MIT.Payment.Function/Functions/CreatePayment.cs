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
        private readonly IPaymentAuditProvider _paymentAuditProvider;
        private readonly ISchemeValidator _schemeValidator;

        public CreatePayment(IEventQueueService eventQueueService, IServiceBus serviceBus, IPaymentAuditProvider paymentAuditProvider,  ISchemeValidator schemeValidator)
        {
            _eventQueueService = eventQueueService;
            _schemeValidator = schemeValidator;
            _serviceBus = serviceBus;
            _paymentAuditProvider = paymentAuditProvider;
        }

        [Function("CreatePayment")]
        public async Task Run(
            [QueueTrigger("%PaymentQueueName%", Connection = "QueueConnectionString")] string paymentRequestMsg,
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

            //if (schemeExists)
            //{
                log.LogInformation("Executing Service Bus For Strategic Payments...");

                string message = JsonConvert.SerializeObject(invoiceScheme);

                await _serviceBus.SendServiceBus(message);
            //}

            // Audit the operation
            _paymentAuditProvider.CreatePaymentInstruction(paymentRequestMsg);
        }
    }
}

