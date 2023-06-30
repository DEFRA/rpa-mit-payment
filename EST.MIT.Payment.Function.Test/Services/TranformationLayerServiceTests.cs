using EST.MIT.Payment.Interfaces;
using EST.MIT.Payment.Services;

namespace EST.MIT.Payment.Function.Test.Services;

public class TranformationLayerServiceTests
{
    private readonly IPayment _paymentService;

    public TranformationLayerServiceTests()
    {
        _paymentService = new TranformationLayerService();
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
