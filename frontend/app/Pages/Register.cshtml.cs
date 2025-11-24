using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using app.Services;

namespace app.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IApiService _apiService;
        public string? ErrorMessage { get; set; }

        public RegisterModel(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var email = Request.Form["email"].ToString();
            var password = Request.Form["password"].ToString();
            var confirmPassword = Request.Form["confirmPassword"].ToString();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ErrorMessage = "Email e senha são obrigatórios.";
                return Page();
            }

            if (password != confirmPassword)
            {
                ErrorMessage = "As senhas não correspondem.";
                return Page();
            }

            if (password.Length < 6)
            {
                ErrorMessage = "A senha deve ter pelo menos 6 caracteres.";
                return Page();
            }

            var (success, user, error) = await _apiService.RegisterAsync(email, password, "user");

            if (success && user != null)
            {
                return RedirectToPage("/Login", new { message = "Cadastro realizado com sucesso! Faça login." });
            }

            ErrorMessage = error ?? "Erro ao registrar usuário.";
            return Page();
        }
    }
}
