using EST.MIT.Payment.Models;
using EST.MIT.Payment.Services;

namespace EST.MIT.Payment.Function.Tests.Services;

public class PaymentRequestServiceTests
{
    private readonly PaymentRequestService _paymentRequestService;

    public PaymentRequestServiceTests()
    {
        _paymentRequestService = new PaymentRequestService();
    }

    [Fact]
    public void ValidatePaymentRequest_ValidRequest_ReturnsTrue()
    {
        var paymentRequest = new StrategicPaymentInstruction
        {
            Sbi = 123,
            Value = 1000,
            SchemeId = 1,
            Frn = 123456789,
            MarketingYear = DateTime.Now.Year - 1,
            DueDate = DateTime.Now.AddDays(1),
            PaymentDetails = new List<StrategicPaymentDetail> { new StrategicPaymentDetail
            {
                StandardCode = "StandardCode",
                Description = "Description",
                Value = 1,
                SchemeCode = "SchemeCode",
                FundCode = "FundCode"
            } }
        };

        var result = _paymentRequestService.ValidatePaymentRequest(paymentRequest);

        Assert.True(result);
    }

    [Fact]
    public void ValidatePaymentRequest_InvalidSbi_ReturnsFalse()
    {
        var paymentRequest = new StrategicPaymentInstruction
        {
            Sbi = -1,
            Value = 1000,
            SchemeId = 1,
            Frn = 123456789,
            MarketingYear = DateTime.Now.Year - 1,
            DueDate = DateTime.Now.AddDays(1),
            PaymentDetails = new List<StrategicPaymentDetail> { new StrategicPaymentDetail
            {
                StandardCode = "StandardCode",
                Description = "Description",
                Value = 1,
                SchemeCode = "SchemeCode",
                FundCode = "FundCode"
            } }
        };

        var result = _paymentRequestService.ValidatePaymentRequest(paymentRequest);

        Assert.False(result);
    }

    [Fact]
    public void ValidatePaymentRequest_InvalidValue_ReturnsFalse()
    {
        var paymentRequest = new StrategicPaymentInstruction
        {
            Sbi = 123,
            Value = -1000,
            SchemeId = 1,
            Frn = 123456789,
            MarketingYear = DateTime.Now.Year - 1,
            DueDate = DateTime.Now.AddDays(1),
            PaymentDetails = new List<StrategicPaymentDetail> { new StrategicPaymentDetail
            {
                StandardCode = "StandardCode",
                Description = "Description",
                Value = 1,
                SchemeCode = "SchemeCode",
                FundCode = "FundCode"
            } }
        };

        var result = _paymentRequestService.ValidatePaymentRequest(paymentRequest);

        Assert.False(result);
    }

    [Fact]
    public void ValidatePaymentRequest_InvalidSchemeId_ReturnsFalse()
    {
        var paymentRequest = new StrategicPaymentInstruction
        {
            Sbi = 123,
            Value = 1000,
            SchemeId = -1,
            Frn = 123456789,
            MarketingYear = DateTime.Now.Year - 1,
            DueDate = DateTime.Now.AddDays(1),
            PaymentDetails = new List<StrategicPaymentDetail> { new StrategicPaymentDetail
            {
                StandardCode = "StandardCode",
                Description = "Description",
                Value = 1,
                SchemeCode = "SchemeCode",
                FundCode = "FundCode"
            } }
        };

        var result = _paymentRequestService.ValidatePaymentRequest(paymentRequest);

        Assert.False(result);
    }

    [Fact]
    public void ValidatePaymentRequest_InvalidFrn_ReturnsFalse()
    {
        var paymentRequest = new StrategicPaymentInstruction
        {
            Sbi = 123,
            Value = 1000,
            SchemeId = 1,
            Frn = -1,
            MarketingYear = DateTime.Now.Year - 1,
            DueDate = DateTime.Now.AddDays(1),
            PaymentDetails = new List<StrategicPaymentDetail> { new StrategicPaymentDetail
            {
                StandardCode = "StandardCode",
                Description = "Description",
                Value = 1,
                SchemeCode = "SchemeCode",
                FundCode = "FundCode"
            } }
        };

        var result = _paymentRequestService.ValidatePaymentRequest(paymentRequest);

        Assert.False(result);
    }

    [Fact]
    public void ValidatePaymentRequest_InvalidMarketingYearCurrent_ReturnsFalse()
    {
        var paymentRequest = new StrategicPaymentInstruction
        {
            Sbi = 123,
            Value = 1000,
            SchemeId = 1,
            Frn = 123456789,
            MarketingYear = DateTime.Now.Year,
            DueDate = DateTime.Now.AddDays(1),
            PaymentDetails = new List<StrategicPaymentDetail> { new StrategicPaymentDetail
            {
                StandardCode = "StandardCode",
                Description = "Description",
                Value = 1,
                SchemeCode = "SchemeCode",
                FundCode = "FundCode"
            } }
        };

        var result = _paymentRequestService.ValidatePaymentRequest(paymentRequest);

        Assert.False(result);
    }

    [Fact]
    public void ValidatePaymentRequest_InvalidMarketingYearFuture_ReturnsFalse()
    {
        var paymentRequest = new StrategicPaymentInstruction
        {
            Sbi = 123,
            Value = 1000,
            SchemeId = 1,
            Frn = 123456789,
            MarketingYear = DateTime.Now.Year + 1,
            DueDate = DateTime.Now.AddDays(1),
            PaymentDetails = new List<StrategicPaymentDetail> { new StrategicPaymentDetail
            {
                StandardCode = "StandardCode",
                Description = "Description",
                Value = 1,
                SchemeCode = "SchemeCode",
                FundCode = "FundCode"
            } }
        };

        var result = _paymentRequestService.ValidatePaymentRequest(paymentRequest);

        Assert.False(result);
    }

    [Fact]
    public void ValidatePaymentRequest_InvalidDueDate_ReturnsFalse()
    {
        var paymentRequest = new StrategicPaymentInstruction
        {
            Sbi = 123,
            Value = 1000,
            SchemeId = 1,
            Frn = 123456789,
            MarketingYear = DateTime.Now.Year - 1,
            DueDate = DateTime.Now.AddDays(-1),
            PaymentDetails = new List<StrategicPaymentDetail> { new StrategicPaymentDetail
            {
                StandardCode = "StandardCode",
                Description = "Description",
                Value = 1,
                SchemeCode = "SchemeCode",
                FundCode = "FundCode"
            } }
        };

        var result = _paymentRequestService.ValidatePaymentRequest(paymentRequest);

        Assert.False(result);
    }

    [Fact]
    public void ValidatePaymentRequest_EmptyPaymentDetailsList_ReturnsFalse()
    {
        var paymentRequest = new StrategicPaymentInstruction
        {
            Sbi = 123,
            Value = 1000,
            SchemeId = 1,
            Frn = 123456789,
            MarketingYear = DateTime.Now.Year - 1,
            DueDate = DateTime.Now.AddDays(1),
            PaymentDetails = new List<StrategicPaymentDetail>()
        };

        var result = _paymentRequestService.ValidatePaymentRequest(paymentRequest);
        Assert.False(result);
    }


    [Fact]
    public void ValidatePaymentRequest_NullPaymentDetailsList_ReturnsFalse()
    {
        var paymentRequest = new StrategicPaymentInstruction
        {
            Sbi = 123,
            Value = 1000,
            SchemeId = 1,
            Frn = 123456789,
            MarketingYear = DateTime.Now.Year - 1,
            DueDate = DateTime.Now.AddDays(1),
            PaymentDetails = null
        };

        var result = _paymentRequestService.ValidatePaymentRequest(paymentRequest);
        Assert.False(result);
    }

    [Fact]
    public void ValidatePaymentRequest_InvalidInvoiceLinesStandardCode_ReturnsFalse()
    {
        var paymentRequest = new StrategicPaymentInstruction
        {
            Sbi = 123,
            Value = 1000,
            SchemeId = 1,
            Frn = 123456789,
            MarketingYear = DateTime.Now.Year - 1,
            DueDate = DateTime.Now.AddDays(1),
            PaymentDetails = new List<StrategicPaymentDetail>
            {
                new StrategicPaymentDetail
                {
                    StandardCode = "StandardCode",
                    Description = "Description",
                    Value = 1,
                    SchemeCode = "SchemeCode",
                    FundCode = ""
                }
            }
        };

        var result = _paymentRequestService.ValidatePaymentRequest(paymentRequest);

        Assert.False(result);
    }

    [Fact]
    public void ValidatePaymentRequest_InvalidInvoiceLinesDescription_ReturnsFalse()
    {
        var paymentRequest = new StrategicPaymentInstruction
        {
            Sbi = 123,
            Value = 1000,
            SchemeId = 1,
            Frn = 123456789,
            MarketingYear = DateTime.Now.Year - 1,
            DueDate = DateTime.Now.AddDays(1),
            PaymentDetails = new List<StrategicPaymentDetail>
            {
                new StrategicPaymentDetail
                {
                    StandardCode = "StandardCode",
                    Description = "Description",
                    Value = 1,
                    SchemeCode = "SchemeCode",
                    FundCode = ""
                }
            }
        };

        var result = _paymentRequestService.ValidatePaymentRequest(paymentRequest);

        Assert.False(result);
    }

    [Fact]
    public void ValidatePaymentRequest_InvalidInvoiceLinesSchemeCode_ReturnsFalse()
    {
        var paymentRequest = new StrategicPaymentInstruction
        {
            Sbi = 123,
            Value = 1000,
            SchemeId = 1,
            Frn = 123456789,
            MarketingYear = DateTime.Now.Year - 1,
            DueDate = DateTime.Now.AddDays(1),
            PaymentDetails = new List<StrategicPaymentDetail>
            {
                new StrategicPaymentDetail
                {
                    StandardCode = "StandardCode",
                    Description = "Description",
                    Value = 1,
                    SchemeCode = "SchemeCode",
                    FundCode = ""
                }
            }
        };

        var result = _paymentRequestService.ValidatePaymentRequest(paymentRequest);

        Assert.False(result);
    }

    [Fact]
    public void ValidatePaymentRequest_InvalidInvoiceLinesFundCode_ReturnsFalse()
    {
        var paymentRequest = new StrategicPaymentInstruction
        {
            Sbi = 123,
            Value = 1000,
            SchemeId = 1,
            Frn = 123456789,
            MarketingYear = DateTime.Now.Year - 1,
            DueDate = DateTime.Now.AddDays(1),
            PaymentDetails = new List<StrategicPaymentDetail>
            {
                new StrategicPaymentDetail
                {
                    StandardCode = "StandardCode",
                    Description = "Description",
                    Value = 1,
                    SchemeCode = "SchemeCode",
                    FundCode = ""
                }
            }
        };

        var result = _paymentRequestService.ValidatePaymentRequest(paymentRequest);

        Assert.False(result);
    }

    [Fact]
    public void ValidatePaymentRequest_NegativeValueInPaymentDetail_ReturnsFalse()
    {
        var paymentRequest = new StrategicPaymentInstruction
        {
            PaymentDetails = new List<StrategicPaymentDetail>
            {
                new StrategicPaymentDetail
                {
                    StandardCode = "StandardCode",
                    Description = "Description",
                    Value = -1,
                    SchemeCode = "SchemeCode",
                    FundCode = "FundCode"
                }
            }
        };

        var result = _paymentRequestService.ValidatePaymentRequest(paymentRequest);

        Assert.False(result);
    }

    [Fact]
    public void ValidatePaymentRequest_MultiplePaymentDetailsOneInvalid_ReturnsFalse()
    {
        var paymentRequest = new StrategicPaymentInstruction
        {
            PaymentDetails = new List<StrategicPaymentDetail>
            {
                new StrategicPaymentDetail
                {
                    StandardCode = "StandardCode",
                    Description = "Description",
                    Value = 1,
                    SchemeCode = "SchemeCode",
                    FundCode = "FundCode"
                } ,
                new StrategicPaymentDetail
                {
                    StandardCode = "StandardCode",
                    Description = "Description",
                    Value = -1,
                    SchemeCode = "SchemeCode",
                    FundCode = "FundCode"
                }
            }
        };

        var result = _paymentRequestService.ValidatePaymentRequest(paymentRequest);

        Assert.False(result);
    }


    [Fact]
    public void ValidatePaymentRequest_NullRequest_ReturnsFalse()
    {
        var result = _paymentRequestService.ValidatePaymentRequest(null);
        Assert.False(result);
    }

}
