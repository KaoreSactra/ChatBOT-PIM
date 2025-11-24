using api_back.Data;
using api_back.Models;
using Microsoft.EntityFrameworkCore;
using Hasher = BCrypt.Net.BCrypt;
using DotNetEnv;

// Tentar carregar .env de vários locais possíveis
var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
if (!File.Exists(envPath))
{
    // Tentar no diretório pai (para desenvolvimento)
    envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".env");
}
if (File.Exists(envPath))
{
    DotNetEnv.Env.Load(envPath);
}

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApiDbContext>(options =>
{
    // Usar In-Memory Database para desenvolvimento
    options.UseInMemoryDatabase("TestDb");
});

// Adicionar HttpClient
builder.Services.AddHttpClient();

// Adicionar In-Memory Cache (necessário para Session)
builder.Services.AddDistributedMemoryCache();

// Adicionar Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Adicionar CORS para permitir requisições do frontend
builder.Services.AddCors(options =>
{
    // Obter URLs permitidas das variáveis de ambiente
    var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "http://localhost:6661";
    
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(frontendUrl, "http://localhost:6661")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
              .WithExposedHeaders("*");
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Criar banco de dados automaticamente e adicionar seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
    db.Database.EnsureCreated();
    
    // Seed: Criar usuário admin se não existir
    if (!db.Users.Any(u => u.Email == "admin@admin.com"))
    {
        var adminUser = new User
        {
            Email = "admin@admin.com",
            PasswordHash = Hasher.HashPassword("admin"),
            Role = "admin"
        };
        db.Users.Add(adminUser);
        db.SaveChanges();
        Console.WriteLine("✅ Usuário admin criado: admin@admin.com / admin");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Usar CORS ANTES de UseRouting
app.UseCors("AllowAll");

app.UseRouting();

app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

// Health check endpoint
app.MapGet("/health", () => new { status = "OK", timestamp = DateTime.UtcNow })
    .WithName("Health")
    .WithOpenApi();

app.MapControllers();

app.Run();
