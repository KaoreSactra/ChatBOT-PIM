# ⚡ Solução Rápida - Connection Refused 192.168.1.113:6660

## 🔴 Problema

```
Connection refused (192.168.1.113:6660)
```

O backend/frontend não estão escutando no IP da rede.

## 🟢 Solução

### ❌ **NÃO faça isto:**

```bash
cd backend && dotnet run     # SEM as variáveis de ambiente
```

### ✅ **FAÇA isto:**

#### **Opção 1: Usar o script startup (RECOMENDADO)**

```bash
# Linux/macOS
./startup.sh
# Escolha opção 3

# Windows
startup.bat
REM Escolha opção 3
```

#### **Opção 2: Iniciar manualmente com as variáveis corretas**

**Terminal 1 - Backend:**

```bash
cd backend
export ASPNETCORE_URLS="http://0.0.0.0:6660"
dotnet run
```

**Terminal 2 - Frontend:**

```bash
cd frontend/app
export ASPNETCORE_URLS="http://0.0.0.0:6661"
dotnet run
```

## 🔍 Como Verificar

Abra outro terminal e teste:

```bash
# Testar backend
curl http://192.168.1.113:6660/health

# Deve responder com:
# {"status":"OK","timestamp":"..."}
```

## 📋 Por que isso acontece?

- `ASPNETCORE_URLS="http://0.0.0.0:6660"` configura para escutar em **TODAS as interfaces** de rede
- Sem isso, o .NET escuta apenas em `localhost:6660`
- `0.0.0.0` = qualquer IP (localhost + IPs da rede)
- Assim fica acessível em `192.168.1.113:6660` ✅

## 🚀 Próximos Passos

1. **Mate os processos antigos:**

   ```bash
   pkill -f "dotnet run"
   ```

2. **Reinicie com o script:**

   ```bash
   ./startup.sh  # ou startup.bat
   # Escolha opção 3
   ```

3. **Acesse no navegador:**
   ```
   http://192.168.1.113:6661
   ```

Pronto! Deve funcionar agora. ✅
