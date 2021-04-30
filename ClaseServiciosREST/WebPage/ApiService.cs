using Common;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebPage
{
    public class ApiService
    {

        public const string API_URL = "https://claseserviciorestapi.herokuapp.com/";
        //public const string API_URL = "http://localhost:12345/";

        private HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(API_URL) };
        }

        public async Task<string> GetTokenAsync(string nickname, string password)
        {
            var data = $"{{ \"nickname\": \"{nickname}\", \"password\": \"{password}\" }}";
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(API_URL, content);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<MessageResponse[]> GetMessagesAsync()
        {
            return await GetListAsync<MessageResponse>($"messages/receiver/{Data.Nickname}");
        }

        public async Task<UserProfile> GetUserAsync()
        {
            UserProfile user = null;

            if (!string.IsNullOrEmpty(Data.Nickname))
            {
                user = await GetAsync<UserProfile>($"users/{Data.Nickname}");
            }
            return user;
        }

        private async Task<T[]> GetListAsync<T>(string controller)
        {
            T[] result = null;

            HttpResponseMessage response = await _httpClient.GetAsync(controller);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<T[]>(json);
            }

            return result;
        }

        private async Task<T> GetAsync<T>(string controller)
        {
            T result = default;

            HttpResponseMessage response = await _httpClient.GetAsync(controller);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<T>(json);
            }

            return result;
        }
    }
}
