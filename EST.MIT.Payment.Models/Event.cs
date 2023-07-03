using System.Text.Json.Serialization;

namespace EST.MIT.Payment.Models
{
    public class Event
    {
        [JsonPropertyName("name")]
        public string Name { get; init; } = default!;
        [JsonPropertyName("properties")]
        public EventProperties Properties { get; init; } = default!;
    }
}
