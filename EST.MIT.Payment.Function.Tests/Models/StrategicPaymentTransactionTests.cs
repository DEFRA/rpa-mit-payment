using System.Globalization;
using EST.MIT.Payment.Models;

namespace EST.MIT.Payment.Function.Tests.Models;

public class StrategicPaymentTransactionTests
{
    [Fact]
    public void PaymentTransactionT_Get_and_Set()
    {
        var transaction = new StrategicPaymentTransaction();
        var instruction = new StrategicPaymentInstruction
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
        };
        transaction.Accepted = true;
        transaction.paymentInstruction = instruction;

        Assert.Same(instruction, transaction.paymentInstruction);
        Assert.True(transaction.Accepted);
    }
}
