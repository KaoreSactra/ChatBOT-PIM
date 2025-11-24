namespace DesktopSql.Models
{
    // Essa classe deve ter os mesmos campos que sua API espera
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        // Ajuste conforme o que sua API retorna. 
        // Se ela retorna só uma string ou um objeto JSON, precisamos saber.
        // Vou assumir que ela retorna um JSON com token ou mensagem.
        public string Token { get; set; }
        public string Message { get; set; }
    }
}