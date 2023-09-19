using Newtonsoft.Json;

namespace EST.MIT.Payment.Models;

public class InvoiceScheme
{
    [JsonProperty("schemeType")]
    public string SchemeType { get; init; } = default!;
    [JsonProperty("paymentRequestsBatches")]
    public List<PaymentRequestsBatch> PaymentRequestsBatches { get; init; } = default!;
}