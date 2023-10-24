using Azure.Messaging.ServiceBus;
using EST.MIT.Payment.Models;
using EST.MIT.Payment.Services;
using Moq;
using Newtonsoft.Json;

namespace EST.MIT.Payment.Function.Tests.Services
{
    public class ServiceBusTests
    {
        private readonly ServiceBus _serviceBus;
        private readonly Mock<ServiceBusClient> _mockServiceBusClient;
        private readonly Mock<ServiceBusSender> _mockServiceBusSender;

        public ServiceBusTests()
        {
            _mockServiceBusClient = new Mock<ServiceBusClient>();
            _mockServiceBusSender = new Mock<ServiceBusSender>();
            _serviceBus = new ServiceBus("queueName", _mockServiceBusClient.Object);
        }

        [Fact]
        public async Task Test_Create_Service_Bus()
        {
            var paymentRequest = new InvoiceScheme()
            {
                PaymentRequestsBatches = new List<PaymentRequestsBatch>()
                {
                     new PaymentRequestsBatch()
                     {
                        Id = "1",
                        AccountType = "AD",
                        Created = DateTime.Now,
                        CreatedBy = "henry",
                        Organisation = "FGH",
                        PaymentRequests = new List<PaymentRequest>()
                        {
                            new PaymentRequest()
                            {
                                AgreementNumber = "12345",
                                DueDate = "string",
                                FRN = 56789043,
                                InvoiceLines = new List<InvoiceLine>()
                                {
                                    new InvoiceLine()
                                    {                                         
                                         Description = "This is a description",
                                         FundCode = "2ADC",
                                         SchemeCode = "D4ERT",
                                         Value = 3
                                    }
                                },
                                MarketingYear = 2023,
                                PaymentRequestId = "2",
                                PaymentRequestNumber = 34567,
                                SourceSystem = "sourceSystem",
                                Value = 2
                            }
                        },
                        Reference = "123",
                        SchemeType = "AD",
                        Status = "A",
                        Updated = DateTime.Now,
                        UpdatedBy = "henry"
                     }
                },
                SchemeType = "AD"
            };

            _mockServiceBusClient.Setup(x => x.CreateSender(It.IsAny<string>()))
                                            .Returns(_mockServiceBusSender.Object);

            string message = JsonConvert.SerializeObject(paymentRequest);

            ServiceBusMessage serviceBusMessage = new ServiceBusMessage();

            _mockServiceBusSender.Setup(x => x.SendMessageAsync(serviceBusMessage, default)).Returns(Task.CompletedTask);

            await _serviceBus.SendServiceBus(message);
        }
    }
}
