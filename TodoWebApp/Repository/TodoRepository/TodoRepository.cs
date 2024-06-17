
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Newtonsoft.Json;
using System.Text;

namespace TodoWebApp.Repository.TodoRepository
{
    public class TodoRepository : ITodoRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public TodoRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResponse> SearchMyTodo(int userId, string search, int page, int markId)
        {
            string url = StaticEndpoint.BaseUrl + $"/Todo/SearchMyTodo?userId={userId}&search={Uri.EscapeDataString(search)}&page={page}&markId={markId}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _httpClientFactory.CreateClient("CustomHttpClient");
            HttpResponseMessage response = await client.SendAsync(request);

            ApiResponse apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

            return apiResponse;
        }

        public async Task<ApiResponse> SubmitTodo(Todo todo)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, (StaticEndpoint.BaseUrl + "/Todo/SubmitTodo"))
            {
                Content = new StringContent(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json")
            };
            var client = _httpClientFactory.CreateClient("CustomHttpClient");
            HttpResponseMessage response = await client.SendAsync(request);

            ApiResponse apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return apiResponse;
        }

        public async Task<ApiResponse> DeleteTodo(int userId, int todoId)
        {
            string url = StaticEndpoint.BaseUrl + $"/Todo/DeleteMyTodoById?userId={userId}&todoId={todoId}";
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            var client = _httpClientFactory.CreateClient("CustomHttpClient");
            HttpResponseMessage response = await client.SendAsync(request);

            ApiResponse apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return apiResponse;
        }
        public async Task<ApiResponse> GetMarking()
        {
            string url = StaticEndpoint.BaseUrl + $"/Todo/GetMarking";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _httpClientFactory.CreateClient("CustomHttpClient");
            HttpResponseMessage response = await client.SendAsync(request);

            ApiResponse apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return apiResponse;
        }
    }
}
