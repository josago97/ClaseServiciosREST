using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Common
{
    public class MessageRequest
    {
        [JsonPropertyName("receiver")]
        public string Receiver { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("image")]
        public IFormFile Image { get; set; }
    }
}
