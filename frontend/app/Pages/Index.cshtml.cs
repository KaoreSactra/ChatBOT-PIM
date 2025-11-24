using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using app.Services;
using app.Models;
using System.Text.Json;

namespace app.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IApiService _apiService;

    public string? UserEmail { get; set; }
    public bool IsAdmin { get; set; }
    public string? ChatError { get; set; }
    public string? DashboardError { get; set; }

    public IndexModel(ILogger<IndexModel> logger, IApiService apiService)
    {
        _logger = logger;
        _apiService = apiService;
    }

    public IActionResult OnGet()
    {
        // Verificar se o usuário está autenticado
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToPage("/Login");
        }

        UserEmail = HttpContext.Session.GetString("UserEmail");
        var userRole = HttpContext.Session.GetString("UserRole");
        IsAdmin = userRole == "admin";

        return Page();
    }

    public IActionResult OnPost()
    {
        var action = Request.Form["action"].ToString();
        
        if (action == "logout")
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }

        return OnGet();
    }

    public async Task<IActionResult> OnPostSendMessageAsync([FromBody] MessageRequest request)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return new UnauthorizedResult();
        }

        if (string.IsNullOrEmpty(request?.Message))
        {
            return BadRequest(new { error = "Mensagem vazia" });
        }

        var (success, message, error) = await _apiService.SendChatMessageAsync(request.Message);

        if (success)
        {
            return new JsonResult(new { message = message, success = true });
        }

        return BadRequest(new { error = error ?? "Erro ao enviar mensagem" });
    }

    public async Task<IActionResult> OnGetGetUsersAsync()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return new UnauthorizedResult();
        }

        var userRole = HttpContext.Session.GetString("UserRole");
        if (userRole != "admin")
        {
            return new ForbidResult();
        }

        var (success, users, error) = await _apiService.GetAllUsersAsync();

        if (success)
        {
            return new JsonResult(new { users = users, success = true });
        }

        return BadRequest(new { error = error ?? "Erro ao carregar usuários" });
    }

    public async Task<IActionResult> OnPostDeleteUserAsync([FromBody] DeleteUserRequest request)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return new UnauthorizedResult();
        }

        var userRole = HttpContext.Session.GetString("UserRole");
        if (userRole != "admin")
        {
            return new ForbidResult();
        }

        var (success, error) = await _apiService.DeleteUserAsync(request.UserId);

        if (success)
        {
            return new JsonResult(new { success = true });
        }

        return BadRequest(new { error = error ?? "Erro ao deletar usuário" });
    }
}

public class MessageRequest
{
    public string? Message { get; set; }
}

public class DeleteUserRequest
{
    public int UserId { get; set; }
}
