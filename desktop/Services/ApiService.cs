using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DesktopSql.Models;

namespace DesktopSql.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ApiService()
        {
            _httpClient = new HttpClient();
            // Ler URL base do ambiente, com fallback para localhost
            _baseUrl = Environment.GetEnvironmentVariable("FRONTEND_API_BASE_URL") ?? "http://localhost:6660";
        }

        public async Task<(bool success, string message, LoginModel? user)> LoginAsync(string email, string password)
        {
            try
            {
                var loginRequest = new LoginRequest { Email = email, Password = password };
                
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(loginRequest, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/users/login", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse resposta
                    var loginResponse = JsonSerializer.Deserialize<LoginResponse>(
                        responseContent, 
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
                    );

                    if (loginResponse?.Success == true && loginResponse.User != null)
                    {
                        return (true, loginResponse.User.Id, loginResponse.User);
                    }
                    return (false, loginResponse?.Message ?? "Login falhou", null);
                }
                else
                {
                    return (false, $"Erro {response.StatusCode}: {responseContent}", null);
                }
            }
            catch (Exception ex)
            {
                return (false, $"Erro de conexão: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message, LoginModel? user)> RegisterAsync(string email, string password, string confirmPassword)
        {
            try
            {
                if (password != confirmPassword)
                {
                    return (false, "As senhas não coincidem", null);
                }

                var registerRequest = new RegisterRequest 
                { 
                    Email = email, 
                    Password = password,
                    Role = "user"
                };
                
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(registerRequest, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/users/register", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var registerResponse = JsonSerializer.Deserialize<LoginResponse>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
                    );

                    if (registerResponse?.Success == true && registerResponse.User != null)
                    {
                        return (true, "Usuário registrado com sucesso!", registerResponse.User);
                    }
                    return (false, registerResponse?.Message ?? "Registro falhou", null);
                }
                else
                {
                    return (false, $"Erro {response.StatusCode}: {responseContent}", null);
                }
            }
            catch (Exception ex)
            {
                return (false, $"Erro de conexão: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string response)> SendChatMessageAsync(string message, string userId)
        {
            try
            {
                var chatRequest = new { Message = message, UserId = userId };
                
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(chatRequest, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/chat/send", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var chatResponse = JsonSerializer.Deserialize<ChatResponse>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
                    );
                    return (true, chatResponse?.Response ?? "Sem resposta");
                }
                else
                {
                    return (false, $"Erro {response.StatusCode}: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Erro de conexão: {ex.Message}");
            }
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public LoginModel User { get; set; }
    }

    public class ChatResponse
    {
        public bool Success { get; set; }
        public string Response { get; set; }
    }
}