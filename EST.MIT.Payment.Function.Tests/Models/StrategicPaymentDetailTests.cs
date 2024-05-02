using EST.MIT.Payment.Models;

namespace EST.MIT.Payment.Function.Test.Models;

public class StrategicPaymentDetailTests
{
    [Fact]
    public void PaymentDetail_Get_and_Set()
    {
        var paymentDetail = new StrategicPaymentDetail
        {
            StandardCode = "TestStandardCode",
            Description = "TestDescription",
            Value = 12345,
            SchemeCode = "TestSchemeCode",
            FundCode = "TestFundCode"
        };

        Assert.Equal("TestStandardCode", paymentDetail.StandardCode);
        Assert.Equal("TestDescription", paymentDetail.Description);
        Assert.Equal(12345, paymentDetail.Value);
        Assert.Equal("TestSchemeCode", paymentDetail.SchemeCode);
        Assert.Equal("TestFundCode", paymentDetail.FundCode);
    }
}
