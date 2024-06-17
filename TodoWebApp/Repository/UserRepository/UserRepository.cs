using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace TodoWebApp.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public UserRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResponse> Register(User user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, (StaticEndpoint.BaseUrl + "/User/registerUser"))
            {
                Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
            };
            var client = _httpClientFactory.CreateClient("CustomHttpClient");
            HttpResponseMessage response = await client.SendAsync(request);

            ApiResponse apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return apiResponse;
        }

        public async Task<ApiResponse> Login(User user)
        {
            string url = StaticEndpoint.BaseUrl + $"/User/login?username={Uri.EscapeDataString(user.Username)}&password={Uri.EscapeDataString(user.Password)}";

            // Create the HttpRequestMessage
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _httpClientFactory.CreateClient("CustomHttpClient");
            HttpResponseMessage response = await client.SendAsync(request);


            ApiResponse apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

            return apiResponse;
        }
    }
}
