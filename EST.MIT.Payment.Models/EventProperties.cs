using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EST.MIT.Payment.Models
{
    public class EventProperties
    {
        [JsonPropertyName("checkpoint")]
        public string Checkpoint { get; init; } = default!;
        [JsonPropertyName("status")]
        public string Status { get; init; } = default!;
        [JsonPropertyName("action")]
        public EventAction Action { get; init; } = default!;
    }
}
