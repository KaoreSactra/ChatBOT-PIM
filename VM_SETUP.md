# 🖥️ Configuração para VM - ChatBOT-PIM

## Problema

NetworkError ao acessar o backend de fora da localhost em uma máquina virtual.

## Solução

O projeto foi configurado para funcionar em máquinas virtuais com acesso via IP.

## 📋 Como Usar

### 1. Editar o arquivo `.env` na raiz do projeto

```bash
# Substitua 192.168.1.113 pelo IP da sua VM
BACKEND_URL=http://192.168.1.113:6660
BACKEND_PORT=6660
FRONTEND_API_BASE_URL=http://192.168.1.113:6660

FRONTEND_URL=http://192.168.1.113:6661
FRONTEND_PORT=6661
```

### 2. Iniciar os projetos

**Linux/macOS:**

```bash
./startup.sh
# Escolha opção 3
```

**Windows:**

```cmd
startup.bat
REM Escolha opção 3
```

### 3. Acessar de outra máquina

```
Frontend:  http://192.168.1.113:6661
Backend:   http://192.168.1.113:6660/swagger
```

## 🔧 Configuração Automática

Os scripts agora:

- ✅ Detectam automaticamente o IP da VM (Linux)
- ✅ Configuram `ASPNETCORE_URLS=http://0.0.0.0:PORTA` para escutar em todas as interfaces
- ✅ Suportam acesso via IP e localhost
- ✅ Criam `.env` automaticamente com IP detectado

## ⚠️ Se Receber "NetworkError"

1. **Verifique o IP da VM:**

   ```bash
   hostname -I  # Linux
   ipconfig     # Windows
   ```

2. **Atualize o `.env` com o IP correto:**

   ```
   BACKEND_URL=http://SEU_IP:6660
   FRONTEND_API_BASE_URL=http://SEU_IP:6660
   FRONTEND_URL=http://SEU_IP:6661
   ```

3. **Verifique firewall/conectividade:**

   ```bash
   ping 192.168.1.113
   curl http://192.168.1.113:6660/health
   ```

4. **Reinicie os projetos:**
   ```bash
   ./startup.sh  # opção 5 (limpar) depois opção 3
   ```

## 🌐 Acesso Remoto

Para acessar de outro PC na rede:

1. Descubra o IP da VM: `hostname -I` (Linux) ou `ipconfig` (Windows)
2. No navegador de outro PC: `http://IP_DA_VM:6661`
3. Se não conseguir: verifique firewall e certifique-se que backend está respondendo

## 🚀 Dicas

- Os scripts agora suportam automaticamente localhost E IP da VM
- Para desenvolvimento local: use `localhost:6661`
- Para acesso remoto: use `IP_DA_VM:6661`
- Sempre use HTTP em desenvolvimento (não HTTPS)
