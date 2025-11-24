using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DesktopSql.Models; // Importando a pasta Models

namespace DesktopSql.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        // URL base do seu backend (conforme seu .env)
        private const string BaseUrl = "http://localhost:5210/api";

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var loginData = new LoginRequest { Email = email, Password = password };

            // CONFIGURAÇÃO CRÍTICA: Transforma "Email" em "email" e "Password" em "password"
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            // Passamos as 'options' aqui
            var json = JsonSerializer.Serialize(loginData, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{BaseUrl}/users/login", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    // Isso vai te ajudar a ver o erro real se falhar
                    var erro = await response.Content.ReadAsStringAsync();
                    System.Windows.MessageBox.Show($"Falha no login: {erro}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erro de conexão: {ex.Message}");
                return false;
            }
        }
    }
}