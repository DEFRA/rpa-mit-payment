using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EST.MIT.Payment.Models
{
    public class EventAction
    {
        [JsonPropertyName("type")]
        public string Type { get; init; } = default!;
        [JsonPropertyName("message")]
        public string Message { get; init; } = default!;
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; init; } = default!;
        [JsonPropertyName("data")]
        public string Data { get; init; } = default!;
    }
}
