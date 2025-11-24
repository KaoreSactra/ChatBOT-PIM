# 🔧 Guia de Troubleshooting - ChatBOT-PIM

## ❌ Erro: NetworkError when attempting to fetch resource

Este é um erro de conexão entre o frontend e o backend. Siga os passos abaixo para diagnosticar e resolver.

### 1️⃣ Verificar se o Backend está rodando

```bash
# Linux/macOS
curl http://localhost:6660/health

# Windows (PowerShell)
Invoke-WebRequest http://localhost:6660/health
```

**Esperado:** Resposta JSON com `status: OK`

Se não funcionar:

- Backend não iniciou corretamente
- Porta 6660 está bloqueada ou em uso
- Vá para **[Passo 2](#2️⃣-verificar-se-a-porta-está-disponível)**

### 2️⃣ Verificar se a porta está disponível

#### No Windows:

```cmd
netstat -ano | findstr :6660
```

#### No Linux/macOS:

```bash
lsof -i :6660
```

Se aparecer um processo usando a porta:

- Feche o processo ou altere a porta no `.env`

### 3️⃣ Verificar arquivo `.env`

Confirme que existe um arquivo `.env` **na raiz do projeto** com estas variáveis:

```env
BACKEND_URL=http://localhost:6660
BACKEND_PORT=6660
FRONTEND_API_BASE_URL=http://localhost:6660
FRONTEND_URL=http://localhost:6661
FRONTEND_PORT=6661
GOOGLE_GEMINI_API_KEY=sua_chave_aqui
```

### 4️⃣ Verificar CORS no Backend

O backend deve permitir requisições do frontend. Verifique `backend/Program.cs`:

```csharp
var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "http://localhost:6661";

options.AddPolicy("AllowAll", policy =>
{
    policy.WithOrigins(frontendUrl, "http://localhost:6661")
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials()
          .WithExposedHeaders("*");
});
```

Se o frontend está em outra porta, adicione ao `WithOrigins()`.

### 5️⃣ Verificar se HTTPS é o culpado

Se está em desenvolvimento (`ASPNETCORE_ENVIRONMENT=Development`), HTTPS não deve redirecionar.

- **Frontend:** Deve ter `app.UseHttpsRedirection()` apenas em produção ✅ (já corrigido)
- **Backend:** Deve ter `app.UseHttpsRedirection()` apenas em produção ✅ (já corrigido)

### 6️⃣ Limpar cache do navegador

Às vezes o navegador cache requisições antigas:

1. Abra DevTools (`F12` no Chrome/Firefox)
2. Vá em **Application** → **Storage** → Limpe tudo
3. Ou use **Ctrl+Shift+Del** para limpar cache completo

### 7️⃣ Verificar logs do navegador

No navegador:

1. Abra DevTools (`F12`)
2. Vá em **Console**
3. Vá em **Network**
4. Faça a ação que gera o erro
5. Procure pela requisição que falhou (cor vermelha)
6. Clique nela e veja o status e resposta

### 8️⃣ Rodar os scripts corretamente

#### ✅ Forma Correta:

**No Windows:**

```cmd
startup.bat
```

**No Linux/macOS:**

```bash
chmod +x startup.sh
./startup.sh
```

Depois escolha opção **3** para iniciar ambos (Backend + Frontend)

#### ❌ Forma Incorreta:

- Executar `startup.sh` no Windows (use `startup.bat` em vez disso)
- Rodar apenas o backend ou frontend (precisa de ambos)
- Fechar um dos processos e tentar acessar a URL

### 9️⃣ Teste Manual

Se tudo falhar, teste manualmente:

**Terminal 1 - Backend:**

```bash
cd backend
dotnet run
```

Verá: `Now listening on: http://0.0.0.0:6660`

**Terminal 2 - Frontend:**

```bash
cd frontend/app
dotnet run
```

Verá: `Now listening on: http://0.0.0.0:6661`

**Navegador:**
Acesse `http://localhost:6661` e teste o login

## 📋 Checklist Rápido

- [ ] Backend está rodando na porta 6660?
- [ ] Frontend está rodando na porta 6661?
- [ ] Arquivo `.env` existe na raiz com `FRONTEND_API_BASE_URL=http://localhost:6660`?
- [ ] CORS está configurado corretamente?
- [ ] Cache do navegador foi limpo?
- [ ] Está em desenvolvimento (não produção)?
- [ ] Logs do navegador não mostram erros diferentes?

## 🚀 Se Tudo Falhar

Limpe e reconfigure do zero:

```bash
# Opção 5 no script startup
./startup.sh  # ou startup.bat no Windows
# Escolha: 5 (Limpar e reinstalar dependências)
```

Depois reinicie ambos os projetos (opção 3).

## 📞 Debug Avançado

Se o erro persiste, adicione logs detalhados no `ApiService.cs`:

```csharp
public async Task<(bool success, UserResponse? user, string? error)> LoginAsync(string email, string password)
{
    try
    {
        Console.WriteLine($"[DEBUG] Tentando login em: {_httpClient.BaseAddress}/api/users/login");
        var request = new LoginRequest { Email = email, Password = password };
        var response = await _httpClient.PostAsJsonAsync("/api/users/login", request);

        Console.WriteLine($"[DEBUG] Status: {response.StatusCode}");
        // ... resto do código
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] Erro na requisição: {ex.Message}");
        return (false, null, ex.Message);
    }
}
```
