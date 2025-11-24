using app.Services;
using DotNetEnv;

// Tentar carregar .env de vários locais possíveis
var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
if (!File.Exists(envPath))
{
    // Tentar no diretório pai (para desenvolvimento)
    envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", ".env");
}
if (File.Exists(envPath))
{
    DotNetEnv.Env.Load(envPath);
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Adicionar HttpClient com BaseAddress
var apiBaseUrl = Environment.GetEnvironmentVariable("FRONTEND_API_BASE_URL") ?? "http://localhost:5000";
builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// Adicionar autenticação com sessão
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Adicionar sessão antes de autenticação e autorização
app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
