using Microsoft.Extensions.Logging;
using Moq;
using EST.MIT.Payment.Models;
using EST.MIT.Payment.Function.Functions;

namespace EST.MIT.Payment.Function.Test.Functions;

public class TransformationLayerTests
{
    private readonly Mock<ILogger> _mockLogger;
    private readonly InvoiceScheme _invoiceScheme;

    public TransformationLayerTests()
    {
        _mockLogger = new Mock<ILogger>();
        _invoiceScheme = new InvoiceScheme
        {
            SchemeType = "TestScheme"
        };
    }

    [Fact]
    public async Task ExecuteTransformationLayer_LogsInformation()
    {
        await TransformationLayer.ExecuteTransformationLayer(_invoiceScheme, _mockLogger.Object);

        _mockLogger.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }
}
