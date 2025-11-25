using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using app.Services;

namespace app.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IApiService _apiService;
        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        public LoginModel(IApiService apiService)
        {
            _apiService = apiService;
        }

        public void OnGet()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var email = Request.Form["email"].ToString();
            var password = Request.Form["password"].ToString();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ErrorMessage = "Email e senha são obrigatórios.";
                return Page();
            }

            var (success, user, error) = await _apiService.LoginAsync(email, password);

            if (success && user != null && !string.IsNullOrEmpty(user.Email))
            {
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserRole", user.Role ?? "user");
                
                return RedirectToPage("/Index");
            }

            ErrorMessage = error ?? "Erro ao fazer login. Verifique suas credenciais ou se a API está disponível.";
            return Page();
        }
    }
}
