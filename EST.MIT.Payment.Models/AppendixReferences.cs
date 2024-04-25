using Newtonsoft.Json;

namespace EST.MIT.Payment.Models;

public class AppendixReferences
{
    [JsonProperty("claimReferenceNumber")]
    public string ClaimReferenceNumber { get; init; } = default!;
}