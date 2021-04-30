using System.Text.Json.Serialization;

namespace Common
{
    public class UserProfile
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nickname")]
        public string Nickname { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("url_page")]
        public string UrlPage { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }
    }
}
