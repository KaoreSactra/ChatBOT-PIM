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
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            
            // Ler URL base do ambiente, com fallback para localhost
            var envUrl = Environment.GetEnvironmentVariable("FRONTEND_API_BASE_URL");
            
            System.Diagnostics.Debug.WriteLine($"[ApiService] FRONTEND_API_BASE_URL from env: '{envUrl}'");
            
            // Se vazio, nulo ou só espaços, usar localhost
            if (string.IsNullOrWhiteSpace(envUrl))
            {
                _baseUrl = "http://localhost:6660";
                System.Diagnostics.Debug.WriteLine($"[ApiService] Env var vazia, usando localhost");
            }
            else
            {
                _baseUrl = envUrl.Trim();
                // Garantir que tem http://
                if (!_baseUrl.StartsWith("http://") && !_baseUrl.StartsWith("https://"))
                {
                    _baseUrl = "http://" + _baseUrl;
                }
            }
            
            // Validar se a URL é válida
            try
            {
                var uri = new Uri(_baseUrl);
                System.Diagnostics.Debug.WriteLine($"[ApiService] Base URL validada: {_baseUrl}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ApiService] ERRO ao validar URL '{_baseUrl}': {ex.Message}");
                _baseUrl = "http://localhost:6660";
                System.Diagnostics.Debug.WriteLine($"[ApiService] Usando fallback: {_baseUrl}");
            }
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

                var uri = $"{_baseUrl}/api/users/login";
                System.Diagnostics.Debug.WriteLine($"[ApiService] Login URI: {uri}");
                
                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($"[ApiService] Login Response: {response.StatusCode} - {responseContent}");


                if (response.IsSuccessStatusCode)
                {
                    // Parse resposta
                    var loginResponse = JsonSerializer.Deserialize<LoginResponse>(
                        responseContent, 
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
                    );

                    if (loginResponse?.Success == true && loginResponse.User != null)
                    {
                        return (true, loginResponse.Message ?? "Login realizado com sucesso", loginResponse.User);
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