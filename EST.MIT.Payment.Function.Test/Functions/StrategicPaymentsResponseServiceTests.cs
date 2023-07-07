using Azure.Messaging.ServiceBus;
using EST.MIT.Payment.Function.Functions;
using EST.MIT.Payment.Models;
using EST.MIT.Payment.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace EST.MIT.Payment.Function.Test.Functions
{
    public class StrategicPaymentsResponseServiceTests
    {
        private readonly ServiceBus _serviceBus;
        private readonly Mock<ILogger> _mockLogger;
        private readonly InvoiceScheme _invoiceScheme;
        private readonly Mock<ServiceBusClient> _mockServiceBusClient;
        private readonly Mock<ServiceBusSender> _mockServiceBusSender;
        private readonly StrategicPaymentsResponseService _strategicPaymentsResponseService;
        private readonly Mock<ServiceBusMessage> _mockServiceBusMessage;

        public StrategicPaymentsResponseServiceTests()
        {
            _mockLogger = new Mock<ILogger>();
            _mockServiceBusClient = new Mock<ServiceBusClient>();
            _mockServiceBusSender = new Mock<ServiceBusSender>();
            _mockServiceBusMessage = new Mock<ServiceBusMessage>();
            _serviceBus = new ServiceBus("paymentgeneratorqueue", _mockServiceBusClient.Object, _mockServiceBusMessage.Object);
            _strategicPaymentsResponseService = new StrategicPaymentsResponseService(_serviceBus);

            _invoiceScheme = new InvoiceScheme()
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
        }

        [Fact]
        public async void ServiceBusException_Thrown_When_ServiceBusFailureReason_Is_MessagingEntityNotFound()
        {
            _mockServiceBusClient.Setup(x => x.CreateSender(It.IsAny<string>())).Returns(_mockServiceBusSender.Object);

            _mockServiceBusSender.Setup(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default)).ThrowsAsync(new ServiceBusException("EntityNotFound", ServiceBusFailureReason.MessagingEntityNotFound));

            await _strategicPaymentsResponseService.ExecuteServiceBusForSPS(_invoiceScheme, _mockLogger.Object);
        }

        [Fact]
        public async void ServiceBusException_Thrown_When_ServiceBusFailureReason_Is_ServiceTimeout()
        {
            _mockServiceBusClient.Setup(x => x.CreateSender(It.IsAny<string>())).Returns(_mockServiceBusSender.Object);

            _mockServiceBusSender.Setup(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default)).ThrowsAsync(new ServiceBusException("ServiceTimeOut", ServiceBusFailureReason.ServiceTimeout));

            await _strategicPaymentsResponseService.ExecuteServiceBusForSPS(_invoiceScheme, _mockLogger.Object);
        }

        [Fact]
        public async void ServiceBusException_Thrown_When_ServiceBusFailureReason_Is_MessageSizeExceeded()
        {
            _mockServiceBusClient.Setup(x => x.CreateSender(It.IsAny<string>())).Returns(_mockServiceBusSender.Object);

            _mockServiceBusSender.Setup(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default)).ThrowsAsync(new ServiceBusException("Servi", ServiceBusFailureReason.MessageSizeExceeded));

            await _strategicPaymentsResponseService.ExecuteServiceBusForSPS(_invoiceScheme, _mockLogger.Object);
        }
    }
}
