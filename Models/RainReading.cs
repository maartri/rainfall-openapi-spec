using System.Text.Json.Serialization;

namespace rainfall_openapi_spec.Models
{
    public class RainReading
    {
        [JsonPropertyName("@context")]
        public string Context { get; set; }
        [JsonPropertyName("items")]
        public List<Item> Items { get; set; } = null!;
    }
}
