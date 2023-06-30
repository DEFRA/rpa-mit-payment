using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
