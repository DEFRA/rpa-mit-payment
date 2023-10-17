using Newtonsoft.Json;

namespace EST.MIT.Payment.Models;

public class InvoiceLine
{
    [JsonProperty("value")]
    public decimal Value { get; set; }
    [JsonProperty("schemeCode")]
    public string SchemeCode { get; set; } = null!;
    [JsonProperty("description")]
    public string Description { get; set; } = null!;
    [JsonProperty("fundCode")]
    public string FundCode { get; set; } = null!;
    [JsonProperty("mainAccount")]
    public string MainAccount { get; set; } = null!;
    [JsonProperty("marketingYear")]
    public int MarketingYear { get; set; }
    [JsonProperty("deliveryBody")]
    public string DeliveryBody { get; set; } = null!;
}
