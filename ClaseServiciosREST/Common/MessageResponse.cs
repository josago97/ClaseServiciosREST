using System;
using System.Text.Json.Serialization;

namespace Common
{
    public class MessageResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("receiver")]
        public string Receiver { get; set; }

        [JsonPropertyName("ceated_time")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("edited_time")]
        public DateTime? EditedTime { get; set; }
    }
}
