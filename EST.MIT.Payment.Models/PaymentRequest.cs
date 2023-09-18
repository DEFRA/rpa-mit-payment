using Newtonsoft.Json;

namespace EST.MIT.Payment.Models;
public class PaymentRequest
{
    [JsonProperty("paymentRequestId")]
    public string PaymentRequestId { get; init; } = default!;
    [JsonProperty("frn")]
    public long FRN { get; init; }
    [JsonProperty("sbi")]
    public int SBI { get; init; }
    [JsonProperty("vendor")]
    public string Vendor { get; init; } = default!;
    [JsonProperty("sourceSystem")]
    public string SourceSystem { get; init; } = default!;
    [JsonProperty("marketingYear")]
    public int MarketingYear { get; init; }
    [JsonProperty("currency")]
    public string Currency { get; init; } = default!;   
    [JsonProperty("description")]
    public string Description { get; init; } = default!;
    [JsonProperty("originalInvoiceNumber")]
    public string OriginalInvoiceNumber { get; init; } = default!;  
    [JsonProperty("recoveryDate")]
    public DateTime RecoveryDate { get; init; } = default!; 
    [JsonProperty("invoiceCorrectionReference")]
    public string InvoiceCorrectionReference { get; init; } = default!;  
    [JsonProperty("paymentRequestNumber")]
    public int PaymentRequestNumber { get; init; }
    [JsonProperty("agreementNumber")]
    public string AgreementNumber { get; init; } = default!;
    //[JsonProperty("contractNumber")]
    //public string ContractNumber { get; init; } = default!;
    [JsonProperty("value")]
    public decimal Value { get; init; }
    [JsonProperty("dueDate")]
    public string DueDate { get; init; } = default!;
    [JsonProperty("invoiceLines")]
    public List<InvoiceLine> InvoiceLines { get; init; } = default!;
    //[JsonProperty("appendixReferences")]
    //public AppendixReferences AppendixReferences { get; init; } = default!;
}