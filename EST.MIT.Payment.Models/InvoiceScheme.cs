using Newtonsoft.Json;

namespace EST.MIT.Payment.Models;

public class InvoiceScheme
{
    [JsonProperty("schemeType")]
    public string SchemeType { get; init; } = default!;
    [JsonProperty("invoices")]
    public List<PaymentRequestsBatch> Invoices { get; init; } = default!;
}