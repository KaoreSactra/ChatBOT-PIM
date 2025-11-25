# Network Configuration Guide - ChatBOT-PIM Mobile

## Acessando Backend da Mesma Rede

### Cenário 1: Emulador Android no Mesmo PC

**URL a usar:**

```
http://10.0.2.2:6660
```

O Android Emulator usa `10.0.2.2` para acessar `localhost` da máquina host.

**Em `gradle.properties`:**

```properties
backendUrl=http://10.0.2.2:6660
```

### Cenário 2: Device Real (Celular) Conectado via Wi-Fi

1. **Encontrar o IP do PC/Servidor**

   **Windows (PowerShell/CMD):**

   ```bash
   ipconfig
   ```

   Procure por "IPv4 Address" em sua rede Wi-Fi. Exemplo: `192.168.1.100`

   **Linux/Mac:**

   ```bash
   ifconfig
   # ou
   ip addr show
   ```

2. **Verificar Conectividade**

   ```bash
   # Ping do celular para o servidor (instale app de rede)
   # ou via terminal adb:
   adb shell ping 192.168.1.100
   ```

3. **Atualizar `gradle.properties`**

   ```properties
   # Use o IP encontrado acima
   backendUrl=http://192.168.1.100:6660
   ```

4. **Recompilar**

   ```bash
   ./gradlew clean
   ./gradlew installDebug
   ```

### Cenário 3: Device em Outra Rede

Se o celular está em rede diferente do servidor:

1. **Usar Ngrok para Expor Localmente**

   ```bash
   # Instalar ngrok (https://ngrok.com/download)
   ngrok http 6660
   ```

   Você receberá uma URL pública como: `https://xxxx-xx-xxx-xxx-xx.ngrok.io`

2. **Atualizar `gradle.properties`**

   ```properties
   backendUrl=https://xxxx-xx-xxx-xxx-xx.ngrok.io
   ```

3. **Recompilar e Instalar**

   ```bash
   ./gradlew clean
   ./gradlew installDebug
   ```

## Network Permissions

O app já tem as seguintes permissões no `AndroidManifest.xml`:

```xml
<uses-permission android:name="android.permission.INTERNET" />
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
```

Essas são permissões normais (não requerem aprovação do usuário).

## Firewall e Porta 6660

### Se o App Não Conseguir Conectar:

**1. Verificar se o Backend está Rodando**

```bash
# No servidor C#/.NET
dotnet run

# Você deve ver algo como:
# info: Microsoft.Hosting.Lifetime
#       Now listening on: http://0.0.0.0:6660
```

**2. Testar Conectividade do PC**

```bash
# Verificar se porta 6660 está aberta
netstat -ano | findstr :6660

# Se mostrar LISTENING, está ok
# Se não mostrar, o backend não está rodando
```

**3. Desabilitar Firewall Temporariamente**

```bash
# PowerShell (como Admin)
netsh advfirewall set allprofiles state off

# Para reabilitar depois:
netsh advfirewall set allprofiles state on
```

**4. Abrir Porta no Firewall (Recomendado)**

```bash
# PowerShell (como Admin)
netsh advfirewall firewall add rule name="Backend ChatBOT" dir=in action=allow protocol=tcp localport=6660

# Para remover depois:
netsh advfirewall firewall delete rule name="Backend ChatBOT"
```

## Debugging de Conexão

### Ver Logs do Retrofit no Logcat

Os logs estão habilitados em `ApiClient.kt` com nível `BODY`:

1. **Abrir Logcat:**

   ```
   View > Tool Windows > Logcat
   ```

2. **Filtrar por "api_back" ou "RetrofitService":**

   ```
   Filtro: OkHttp
   ```

3. **Você verá:**
   - URL sendo acessada
   - Headers da requisição
   - Corpo da requisição
   - Status code da resposta
   - Corpo da resposta

### Exemplo de Resposta Bem-Sucedida

```
--> POST http://10.0.2.2:6660/api/users/login
Content-Type: application/json; charset=UTF-8
Content-Length: 49

{"email":"teste@email.com","password":"123456"}
--> END POST

<-- 200 OK http://10.0.2.2:6660/api/users/login (580ms)
Content-Type: application/json; charset=utf-8

{"success":true,"message":"Login realizado com sucesso","user":{"id":1,"email":"teste@email.com","role":"user"}}
<-- END HTTP
```

### Exemplo de Erro de Conexão

```
java.net.ConnectException: Failed to connect to /10.0.2.2:6660
```

**Soluções:**

- Backend não está rodando
- URL errada em `gradle.properties`
- Firewall bloqueando porta 6660

## Performance em Emulador

Se o emulador estiver lento:

1. **Aumentar CPU e RAM alocados**

   - AVD Manager > Editar > Advanced Settings
   - CPU cores: 4+
   - RAM: 2048 MB+

2. **Usar Hardware Acceleration**

   - AVD Manager > Editar > GPU emulation: "Hardware"

3. **Usar Device Real**
   - Muito mais rápido que emulador
   - Conecte via USB com ADB

## Offline Testing

Para testar sem conectar ao backend:

1. **Mock a resposta em `ChatRepository.kt`**

   ```kotlin
   suspend fun sendMessage(messages: List<ChatMessage>): Result<ChatResponse> {
       // Return mock response
       return Result.success(
           ChatResponse(
               success = true,
               message = "Mock response",
               response = "Esta é uma resposta simulada"
           )
       )
   }
   ```

2. **Recompilar e testar**

## Checklist de Conexão

- [ ] Backend rodando em `http://0.0.0.0:6660`
- [ ] Porta 6660 desbloqueada no firewall
- [ ] Device/Emulador pode fazer ping ao servidor
- [ ] URL correta em `gradle.properties` (10.0.2.2 para emulador, IP real para device)
- [ ] INTERNET permission no `AndroidManifest.xml`
- [ ] Recompilou após alterar `gradle.properties`
- [ ] Logcat mostra requisição sendo feita (não erro de conexão)
- [ ] Response HTTP 200 com JSON válido

## Contato

Dúvidas? Verifique:

- Logcat para mensagens de erro
- Backend logs para ver se requisição chegou
- Documentação Android: https://developer.android.com/
