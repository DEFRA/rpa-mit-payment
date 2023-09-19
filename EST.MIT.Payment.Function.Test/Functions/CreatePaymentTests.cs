using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using EST.MIT.Payment.Function.Functions;
using EST.MIT.Payment.Models;
using EST.MIT.Payment.Interfaces;

namespace EST.MIT.Payment.Function.Test;
public class CreatePaymentTests
{
    private readonly CreatePayment _createPayment;
    private readonly Mock<IDurableOrchestrationClient> _mockDurableOrchestrationClient;
    private readonly Mock<IBinder> _mockBinder;
    private readonly Mock<ILogger> _mockLogger;
    private readonly Mock<IEventQueueService> _mockEventQueueService;

    public CreatePaymentTests()
    {
        _mockEventQueueService = new Mock<IEventQueueService>();
        _createPayment = new CreatePayment(_mockEventQueueService.Object);
        _mockDurableOrchestrationClient = new Mock<IDurableOrchestrationClient>();
        _mockBinder = new Mock<IBinder> { CallBase = true };
        _mockLogger = new Mock<ILogger>();
    }

    [Fact]
    public void Given_Function_Receives_Valid_PaymentRequest_Message()
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
                                        DeliveryBody = "DeliveryBody",
                                        MainAccount = "AccountA",
                                        MarketingYear = 2022,
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
        const string functionName = "PaymentOrchestrator";
        const string instanceId = "7E467BDB-213F-407A-B86A-1954053D3C24";

        _mockDurableOrchestrationClient.
           Setup(x => x.StartNewAsync(functionName, It.IsAny<object>())).
            ReturnsAsync(instanceId);

        var result = _createPayment?.Run(message, _mockDurableOrchestrationClient.Object, _mockBinder.Object, _mockLogger.Object);

        _mockLogger.Verify(
    x => x.Log(
        It.IsAny<LogLevel>(),
        It.IsAny<EventId>(),
        It.Is<It.IsAnyType>((v, t) => v.ToString() == $"C# Queue trigger function processed: {message}"),
        It.IsAny<Exception>(),
        It.IsAny<Func<It.IsAnyType, Exception?, string>>()));

        _mockDurableOrchestrationClient.Verify(
            x => x.StartNewAsync(functionName, It.IsAny<object>()),
            Times.Once);

        _mockLogger.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == $"Started orchestration with ID = '{instanceId}'."),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()));


        Assert.True(result?.IsCompletedSuccessfully);
    }

    [Fact]
    public void Given_Function_Receives_InValid_PaymentRequest_Message()
    {
        var InvalidPaymentRequest = new InvoiceScheme()
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
                                    DeliveryBody = "DeliveryBody",
                                    MainAccount = "AccountA",
                                    MarketingYear = 2022,
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
                    }

                }
            }
        };

        string message = JsonConvert.SerializeObject(InvalidPaymentRequest);
        const string functionName = "PaymentOrchestrator";
        const string instanceId = "7E467BDB-213F-407A-B86A-1954053D3C24";

        _mockDurableOrchestrationClient.
           Setup(x => x.StartNewAsync(functionName, It.IsAny<object>())).
            ReturnsAsync(instanceId);

        _createPayment?.Run(message, _mockDurableOrchestrationClient.Object, _mockBinder.Object, _mockLogger.Object);

        _mockLogger.Verify(
    x => x.Log(
    It.IsAny<LogLevel>(),
    It.IsAny<EventId>(),
    It.Is<It.IsAnyType>((v, t) => v.ToString() == $"C# Queue trigger function processed: {message}"),
    It.IsAny<Exception>(),
    It.IsAny<Func<It.IsAnyType, Exception?, string>>()));

        _mockDurableOrchestrationClient.Verify(
            x => x.StartNewAsync(functionName, It.IsAny<object>()),
            Times.Never);
    }

    [Fact]
    public void Given_Function_Receives_Null_PaymentRequest_Message()
    {
        string? message = null;

        _createPayment?.Run(message, _mockDurableOrchestrationClient.Object, _mockBinder.Object, _mockLogger.Object);

        _mockLogger.Verify(
            x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString() == "Payment request is null"),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        _mockDurableOrchestrationClient.Verify(
            x => x.StartNewAsync(It.IsAny<string>(), It.IsAny<object>()),
            Times.Never);
    }

    [Fact]
    public void Given_Function_Receives_Invalid_PaymentRequest_Message()
    {
        string message = "{ 'invalid': 'json' }";

        _createPayment?.Run(message, _mockDurableOrchestrationClient.Object, _mockBinder.Object, _mockLogger.Object);

        _mockLogger.Verify(
            x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString() == "Payment request is not valid"),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        _mockDurableOrchestrationClient.Verify(
            x => x.StartNewAsync(It.IsAny<string>(), It.IsAny<object>()),
            Times.Never);
    }
}
