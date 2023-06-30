using Moq;
using Microsoft.Extensions.Logging;
using EST.MIT.Payment.Models;
using EST.MIT.Payment.Function.Functions;

namespace EST.MIT.Payment.Function.Test.Functions;

public class StrategicPaymentsTests
{
    private readonly Mock<ILogger> _mockLogger;
    private readonly InvoiceScheme _invoiceScheme;

    public StrategicPaymentsTests()
    {
        _mockLogger = new Mock<ILogger>();

        _invoiceScheme = new InvoiceScheme
        {
            SchemeType = "TestScheme"
        };
    }

    [Fact]
    public async Task ExecuteStrategicPayments_DoesNotThrowException()
    {
        Exception? exception = null;

        try
        {
            await StrategicPayments.ExecuteStrategicPayments(_invoiceScheme, _mockLogger.Object);
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.Null(exception);
    }
}
