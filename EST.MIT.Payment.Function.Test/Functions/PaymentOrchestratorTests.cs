using Moq;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using EST.MIT.Payment.Models;
using EST.MIT.Payment.Function.Functions;
using EST.MIT.Payment.Interfaces;

namespace EST.MIT.Payment.Function.Test.Functions;

public class PaymentOrchestratorTests
{
    private readonly Mock<IDurableOrchestrationContext> _mockContext;
    private readonly Mock<ILogger<PaymentOrchestrator>> _mockLogger;
    private readonly InvoiceScheme _invoiceScheme;
    private readonly Mock<ISchemeValidator> _mockSchemeValidator;
    private readonly PaymentOrchestrator _paymentOrchestrator;

    public PaymentOrchestratorTests()
    {
        _mockContext = new Mock<IDurableOrchestrationContext>();
        _mockLogger = new Mock<ILogger<PaymentOrchestrator>>();
        _mockSchemeValidator = new Mock<ISchemeValidator>();

        _invoiceScheme = new InvoiceScheme
        {
            SchemeType = "TestScheme"
        };

        _paymentOrchestrator = new PaymentOrchestrator(_mockSchemeValidator.Object);
    }

    [Fact]
    public async Task RunOrchestrator_ExecutesStrategicPayments_WhenSchemeExists()
    {
        _mockContext.Setup(x => x.GetInput<InvoiceScheme>()).Returns(_invoiceScheme);
        _mockSchemeValidator.Setup(x => x.ValueExists(It.IsAny<string>())).Returns(true);
        await _paymentOrchestrator.RunOrchestrator(_mockContext.Object, _mockLogger.Object);

        _mockContext.Verify(x => x.CallActivityAsync("ExecuteStrategicPayments", _invoiceScheme), Times.Once);
    }

    [Fact]
    public async Task RunOrchestrator_ExecutesTransformationLayer_WhenSchemeDoesNotExist()
    {
        _mockContext.Setup(x => x.GetInput<InvoiceScheme>()).Returns(_invoiceScheme);
        _mockSchemeValidator.Setup(x => x.ValueExists(It.IsAny<string>())).Returns(false);
        await _paymentOrchestrator.RunOrchestrator(_mockContext.Object, _mockLogger.Object);

        _mockContext.Verify(x => x.CallActivityAsync("ExecuteTransformationLayer", _invoiceScheme), Times.Once);
    }
}
