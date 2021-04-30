using System.Text.Json.Serialization;

namespace Common
{
    public class SignInRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("nickname")]
        public string Nickname { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
