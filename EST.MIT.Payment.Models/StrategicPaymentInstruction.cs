using System;
using System.Collections.Generic;

namespace EST.MIT.Payment.Models;

public class StrategicPaymentInstruction
{
    public string SourceSystem { get; set; } = default!;
    public int Sbi { get; set; } = default!;
    public int MarketingYear { get; set; }
    public int PaymentRequestNumber { get; set; }
    public string AgreementNumber { get; set; } = default!;
    public int Value { get; set; }
    public List<StrategicPaymentDetail>? PaymentDetails { get; set; } = default!;
    public Guid CorrelationId { get; set; }
    public int SchemeId { get; set; }
    public string InvoiceNumber { get; set; } = default!;
    public string Ledger { get; set; } = default!;
    public int Frn { get; set; }
    public string DeliveryBody { get; set; } = default!;
    public DateTime DueDate { get; set; }
    public string Currency { get; set; } = default!;
}
