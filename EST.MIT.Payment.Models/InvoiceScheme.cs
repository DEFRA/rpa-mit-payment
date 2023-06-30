using System.Collections.Generic;
using Newtonsoft.Json;

namespace EST.MIT.Payment.Models;

public class InvoiceScheme
{
    [JsonProperty("schemeType")]
    public string SchemeType { get; init; } = default!;
    [JsonProperty("invoices")]
    public List<Invoice> Invoices { get; init; } = default!;
}