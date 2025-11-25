# Guia de Instalação - ChatBOT-PIM Mobile (Android)

## Pré-requisitos

1. **Android Studio** (versão Jellyfish 2023.3.1 ou superior)

   - Download: https://developer.android.com/studio

2. **Java Development Kit (JDK)** versão 17+

   - Geralmente vem com Android Studio

3. **Android SDK**

   - API Level 24+ (Android 7.0+)
   - Instalado automaticamente pelo Android Studio

4. **Emulador Android ou Device Real**
   - Para emulador: Crie um AVD (Android Virtual Device)
   - Para device real: Ative Debug USB no celular

## Instalação e Configuração

### 1. Abrir o Projeto

```bash
# Abrir a pasta mobile em Android Studio
1. Abrir Android Studio
2. File > Open
3. Navegue até: ChatBOT-PIM/mobile
4. Clique em "Open"
```

### 2. Configurar a URL do Backend

Edite o arquivo `gradle.properties` na raiz do projeto:

```properties
# Para EMULADOR (acessa localhost da máquina host)
backendUrl=http://10.0.2.2:6660

# Para DEVICE REAL (ajuste com o IP do seu servidor)
# backendUrl=http://192.168.1.100:6660
```

**Encontrar o IP do servidor:**

```bash
# No servidor Windows (cmd ou PowerShell)
ipconfig

# Procure por "IPv4 Address" na rede local
# Exemplo: 192.168.1.100
```

### 3. Sincronizar Gradle

```bash
# O Android Studio pede para sincronizar automaticamente
# Se não pedir, vá em: File > Sync Now
# Ou execute no terminal:
./gradlew sync
```

### 4. Compilar o Projeto

```bash
# No terminal integrado do Android Studio
./gradlew build

# Ou via interface:
# Build > Build Bundle(s) / APK(s) > Build APKs
```

## Executar o App

### Via Android Studio

1. **Conectar Device ou Iniciar Emulador**

   - Device: Conecte via USB e autorize Debug
   - Emulador: Run > Select Device > Create Virtual Device

2. **Executar**
   - Clique no botão "Run" (Shift + F10)
   - Selecione o dispositivo
   - Aguarde a compilação e instalação

### Via Terminal

```bash
# Compilar e instalar em debug
./gradlew installDebug

# Abrir app (se houver apenas um device/emulador conectado)
adb shell am start -n com.chatbot.pim/.MainActivity
```

### Via APK (Release)

```bash
# Gerar APK release (assinado com chave de debug)
./gradlew assembleRelease

# APK estará em: mobile/app/build/outputs/apk/release/app-release.apk
```

## Troubleshooting

### Erro: "Connection Refused" ou "Conectar em 10.0.2.2:6660"

**Solução:**

- Certifique-se que o backend está rodando: `dotnet run`
- Se usar emulador, a URL padrão `10.0.2.2:6660` (localhost no host)
- Se usar device real, altere para o IP correto em `gradle.properties`

### Erro: "SSL: CERTIFICATE_VERIFY_FAILED"

**Solução:**

- O app usa HTTP (não HTTPS)
- Certifique-se que o backend escuta em HTTP
- No arquivo `AndroidManifest.xml`, já está permitido HTTP para redes locais

### Erro: "Failed to connect to localhost/127.0.0.1:6660"

**Solução:**

- Emulador não consegue acessar `127.0.0.1` ou `localhost`
- Use `10.0.2.2` para acessar a máquina host
- Verifique `gradle.properties`

### Gradle Sync Falha

**Solução:**

```bash
# Limpar cache Gradle
./gradlew clean

# Retentar sincronização
./gradlew sync

# Ou via Android Studio: File > Invalidate Caches > Invalidate and Restart
```

### App Fecha Após Abrir

**Solução:**

- Verifique Logcat (View > Tool Windows > Logcat)
- Procure por erros de conexão ou JsonException
- Confirme a URL do backend em `gradle.properties`

## Estrutura do Projeto

```
mobile/
├── app/
│   ├── src/
│   │   ├── main/
│   │   │   ├── kotlin/com/chatbot/pim/
│   │   │   │   ├── MainActivity.kt              # Ponto de entrada
│   │   │   │   ├── api/
│   │   │   │   │   ├── ApiClient.kt             # Cliente Retrofit
│   │   │   │   │   ├── ApiService.kt            # Endpoints API
│   │   │   │   │   └── models/                  # Data classes
│   │   │   │   ├── repository/
│   │   │   │   │   ├── AuthRepository.kt        # Login/Register
│   │   │   │   │   └── ChatRepository.kt        # Chat
│   │   │   │   ├── viewmodel/
│   │   │   │   │   ├── AuthViewModel.kt         # Lógica Auth
│   │   │   │   │   └── ChatViewModel.kt         # Lógica Chat
│   │   │   │   └── ui/
│   │   │   │       ├── screens/
│   │   │   │       │   ├── AuthScreens.kt       # Login/Register UI
│   │   │   │       │   └── ChatScreen.kt        # Chat UI
│   │   │   │       └── theme/                   # Temas e cores
│   │   │   ├── res/
│   │   │   │   ├── values/
│   │   │   │   └── drawable/
│   │   │   └── AndroidManifest.xml
│   │   └── test/
│   ├── build.gradle.kts
│   └── proguard-rules.pro
├── build.gradle.kts
├── settings.gradle.kts
├── gradle.properties                           # Config backend URL
├── gradlew                                     # Wrapper Linux/Mac
└── gradlew.bat                                 # Wrapper Windows
```

## Tecnologias Utilizadas

- **Kotlin**: Linguagem principal
- **Jetpack Compose**: UI moderno
- **Retrofit**: Requisições HTTP
- **OkHttp**: Cliente HTTP
- **Moshi**: JSON parsing
- **Coroutines**: Programação assíncrona
- **Material Design 3**: Design system

## Endpoints Utilizados

### Login

```
POST /api/users/login
{
  "email": "usuario@email.com",
  "password": "senha"
}
```

### Registro

```
POST /api/users/register
{
  "email": "novo@email.com",
  "password": "senha",
  "role": "user"
}
```

### Chat

```
POST /api/chat
{
  "messages": [
    {
      "role": "user",
      "content": "Olá"
    }
  ]
}
```

## Dicas de Desenvolvimento

1. **Logcat**: Monitore logs em `View > Tool Windows > Logcat`
2. **Device File Explorer**: Acesse arquivos do dispositivo
3. **Profiler**: Analise performance em `View > Tool Windows > Profiler`
4. **Debugger**: Adicione breakpoints (Ctrl + Shift + B) para depurar

## Contato e Suporte

Para dúvidas, consulte:

- Documentação Android: https://developer.android.com/docs
- Documentação Compose: https://developer.android.com/develop/ui/compose
- Retrofit: https://square.github.io/retrofit/
