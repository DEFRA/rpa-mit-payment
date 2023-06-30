using Azure.Messaging.ServiceBus;
using EST.MIT.Payment.Models;
using EST.MIT.Payment.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;

namespace EST.MIT.Payment.Function.Test.Services
{
    public class ServiceBusTests
    {
        private readonly ServiceBus _serviceBus;
        private readonly Mock<ServiceBusClient> _mockServiceBusClient;
        private readonly Mock<ServiceBusSender> _mockServiceBusSender;
        private readonly Mock<IConfiguration> _mockConfiguration;

        public ServiceBusTests()
        {
            _mockServiceBusClient = new Mock<ServiceBusClient>();
            _mockServiceBusSender = new Mock<ServiceBusSender>();
            _mockConfiguration = new Mock<IConfiguration>();
            _serviceBus = new ServiceBus(_mockConfiguration.Object);

            _mockConfiguration.Setup(x => x["ConnectionString"]).Returns("Endpoint=sb://paymentgenerator.servicebus.windows.net/;SharedAccessKeyName=SenderPolicy;SharedAccessKey=eCcIV666vfuLtjU4dtBk0xqS1oZFF7AlT+ASbJo2sV0=");
            _mockConfiguration.Setup(x => x["QueueName"]).Returns("paymentgeneratorqueue");
        }

        [Fact]
        public async Task Test_Create_Service_Bus()
        {
            var paymentRequest = new InvoiceScheme()
            {
                Invoices = new List<Invoice>()
                {
                     new Invoice()
                     {
                        Id = "1",
                        AccountType = "AD",
                        Created = DateTime.Now,
                        CreatedBy = "henry",
                        InvoiceType = "BP",
                        Organisation = "FGH",
                        PaymentRequests = new List<PaymentRequest>()
                        {
                            new PaymentRequest()
                            {
                                AgreementNumber = "12345",
                                AppendixReferences = new AppendixReferences()
                                {
                                     ClaimReferenceNumber = "CCCC"
                                },
                                ContractNumber = "07765432",
                                DeliveryBody = "xyz",
                                DueDate = "string",
                                FRN = 56789043,
                                InvoiceLines = new List<InvoiceLine>()
                                {
                                    new InvoiceLine()
                                    {
                                         Currency = "£",
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

            string message = JsonConvert.SerializeObject(paymentRequest);
            _mockServiceBusClient.Setup(x => x.CreateSender(It.IsAny<string>()))
                                            .Returns(_mockServiceBusSender.Object);

            ServiceBusMessage serviceBusMessage = new ServiceBusMessage();

            _mockServiceBusSender.Setup(x => x.SendMessageAsync(serviceBusMessage, default)).Returns(Task.CompletedTask);

            _mockServiceBusSender.Setup(x => x.DisposeAsync()).Returns(ValueTask.CompletedTask);
            _mockServiceBusClient.Setup(x => x.DisposeAsync()).Returns(ValueTask.CompletedTask);

            await _serviceBus.SendServiceBus(message);
        }
    }
}
