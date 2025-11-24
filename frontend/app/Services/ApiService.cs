using app.Models;
using System.Text.Json;

namespace app.Services
{
    public interface IApiService
    {
        Task<(bool success, UserResponse? user, string? error)> LoginAsync(string email, string password);
        Task<(bool success, UserResponse? user, string? error)> RegisterAsync(string email, string password, string role = "user");
        Task<(bool success, List<UserResponse>? users, string? error)> GetAllUsersAsync();
        Task<(bool success, UserResponse? user, string? error)> GetUserByIdAsync(int id);
        Task<(bool success, string? error)> UpdateUserAsync(int id, string email, string password, string role);
        Task<(bool success, string? error)> DeleteUserAsync(int id);
        Task<(bool success, string? message, string? error)> SendChatMessageAsync(string message);
    }

    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(bool success, UserResponse? user, string? error)> LoginAsync(string email, string password)
        {
            try
            {
                var request = new LoginRequest { Email = email, Password = password };
                var response = await _httpClient.PostAsJsonAsync("/api/users/login", request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var user = JsonSerializer.Deserialize<UserResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return (true, user, null);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return (false, null, errorContent);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool success, UserResponse? user, string? error)> RegisterAsync(string email, string password, string role = "user")
        {
            try
            {
                var request = new RegisterRequest { Email = email, Password = password, Role = role };
                var response = await _httpClient.PostAsJsonAsync("/api/users/register", request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var user = JsonSerializer.Deserialize<UserResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return (true, user, null);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return (false, null, errorContent);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool success, List<UserResponse>? users, string? error)> GetAllUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/users");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var users = JsonSerializer.Deserialize<List<UserResponse>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return (true, users, null);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return (false, null, errorContent);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool success, UserResponse? user, string? error)> GetUserByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/users/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var user = JsonSerializer.Deserialize<UserResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return (true, user, null);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return (false, null, errorContent);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool success, string? error)> UpdateUserAsync(int id, string email, string password, string role)
        {
            try
            {
                var request = new RegisterRequest { Email = email, Password = password, Role = role };
                var response = await _httpClient.PutAsJsonAsync($"/api/users/{id}", request);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return (false, errorContent);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool success, string? error)> DeleteUserAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/users/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return (false, errorContent);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool success, string? message, string? error)> SendChatMessageAsync(string message)
        {
            try
            {
                var request = new ChatRequest { Message = message };
                var response = await _httpClient.PostAsJsonAsync("/api/chat/send", request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var chatResponse = JsonSerializer.Deserialize<ChatResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return (true, chatResponse?.Message, null);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return (false, null, errorContent);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
    }
}
