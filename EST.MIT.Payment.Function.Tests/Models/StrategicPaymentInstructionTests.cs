using System.Globalization;
using EST.MIT.Payment.Models;

namespace EST.MIT.Payment.Function.Tests.Models;

public class StrategicPaymentInstructionTests
{
    [Fact]
    public void PaymentInstruction_Get_and_Set()
    {
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

        Assert.Equal("AHWR", instruction.SourceSystem);
        Assert.Equal(999999999, instruction.Sbi);
        Assert.Equal(2022, instruction.MarketingYear);
        Assert.Equal(1, instruction.PaymentRequestNumber);
        Assert.Equal("VV-6D85-0EC1", instruction.AgreementNumber);
        Assert.Equal(43600, instruction.Value);
        Assert.NotNull(instruction.PaymentDetails);
        Assert.Single(instruction.PaymentDetails);
        Assert.Equal(new Guid("79cf1fd1-6687-488a-8004-95547ec83e52"), instruction.CorrelationId);
        Assert.Equal(4, instruction.SchemeId);
        Assert.Equal("VV-6D85-0EC1V001", instruction.InvoiceNumber);
        Assert.Equal("AP", instruction.Ledger);
        Assert.Equal(1102057452, instruction.Frn);
        Assert.Equal("RP00", instruction.DeliveryBody);

        var expectedDueDate = DateTime.ParseExact("17/06/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture);
        Assert.Equal(expectedDueDate, instruction.DueDate);
    }
}
