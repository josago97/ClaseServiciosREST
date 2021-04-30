using System.Text.Json.Serialization;

namespace Common
{
    public class MessageEditRequest
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
