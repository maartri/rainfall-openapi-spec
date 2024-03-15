using System.Text.Json.Serialization;

namespace rainfall_openapi_spec.Models
{
    public class Item
    {
        [JsonPropertyName("@id")]
        public string Id { get; set; }
        [JsonPropertyName("dateTime")]
        public DateTime DateTime { get; set; }
        [JsonPropertyName("measure")]
        public string Measure { get; set; }
        [JsonPropertyName("value")]
        public decimal Value { get; set; }
    }
}
