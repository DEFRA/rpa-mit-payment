using EST.MIT.Payment.Interfaces;
using EST.MIT.Payment.Services;

namespace EST.MIT.Payment.Function.Test.Services;

public class StrategicPaymentServiceTests
{
    private readonly IPayment _paymentService;

    public StrategicPaymentServiceTests()
    {
        _paymentService = new StrategicPaymentService();
    }

    [Fact]
    public void Generate_Throws_NotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => _paymentService.Generate());
    }

    [Fact]
    public void Send_Throws_NotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => _paymentService.Send());
    }

    [Fact]
    public void Store_Throws_NotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => _paymentService.Store());
    }
}
