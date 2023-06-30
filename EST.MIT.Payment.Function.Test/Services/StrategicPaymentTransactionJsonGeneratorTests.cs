using EST.MIT.Payment.Services;
using EST.MIT.Payment.Interfaces;
using EST.MIT.Payment.Models;
using System.Globalization;

namespace EST.MIT.Payment.Function.Test.Services;

public class StrategicPaymentTransactionJsonGeneratorTests
{
    private readonly IStrategicPaymentTransactionJsonGenerator _generator;

    public StrategicPaymentTransactionJsonGeneratorTests()
    {
        _generator = new StrategicPaymentTransactionJsonGenerator();
    }

    [Fact]
    public void Generate_ReturnsCorrectJson()
    {
        var paymentTransaction = new StrategicPaymentTransaction
        {
            paymentInstruction = new StrategicPaymentInstruction
            {
                SourceSystem = "AHWR",
                Sbi = 999999999,
                MarketingYear = 2022,
                PaymentRequestNumber = 1,
                AgreementNumber = "VV-6D85-0EC1",
                Value = 43600,
                PaymentDetails = new List<StrategicPaymentDetail>
                {
                    new StrategicPaymentDetail
                    {
                        StandardCode = "AHWR-Sheep",
                        Description = "G00 - Gross value of claim",
                        Value = 43600,
                        SchemeCode = "18001",
                        FundCode = "DOM10"
                    }
                },
                CorrelationId = new Guid("79cf1fd1-6687-488a-8004-95547ec83e52"),
                SchemeId = 4,
                InvoiceNumber = "VV-6D85-0EC1V001",
                Ledger = "AP",
                Frn = 1102057452,
                DeliveryBody = "RP00",
                DueDate = DateTime.ParseExact("17/06/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Currency = "GBP"
            },
            Accepted = true
        };

        var expectedJson = @"{""paymentInstruction"":{""sourceSystem"":""AHWR"",""sbi"":999999999,""marketingYear"":2022,""paymentRequestNumber"":1,""agreementNumber"":""VV-6D85-0EC1"",""value"":43600,""paymentDetails"":[{""standardCode"":""AHWR-Sheep"",""description"":""G00 - Gross value of claim"",""value"":43600,""schemeCode"":""18001"",""fundCode"":""DOM10""}],""correlationId"":""79cf1fd1-6687-488a-8004-95547ec83e52"",""schemeId"":4,""invoiceNumber"":""VV-6D85-0EC1V001"",""ledger"":""AP"",""frn"":1102057452,""deliveryBody"":""RP00"",""dueDate"":""2022-06-17T00:00:00"",""currency"":""GBP""},""accepted"":true}";
        var actualJson = _generator.Generate(paymentTransaction);

        Assert.Equal(expectedJson, actualJson);
    }
}
