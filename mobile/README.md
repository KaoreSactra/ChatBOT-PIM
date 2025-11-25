# ChatBOT-PIM Mobile (Android/Kotlin)

App mobile em Kotlin para Android Studio que replica a funcionalidade do app desktop.

## Requisitos

- Android Studio Jellyfish ou superior
- SDK Android 24+ (API level 24 ou superior)
- Kotlin 1.9+
- Gradle 8.0+

## Funcionalidades

- ✅ Login de usuários
- ✅ Registrar novo usuário
- ✅ Chat em tempo real com IA (Google Gemini)
- ✅ Histórico de mensagens
- ✅ Acesso à API backend na rede local

## Configuração

### 1. Definir URL do Backend

No arquivo `local.properties` (criar se não existir):

```properties
# URL do backend (ajuste o IP conforme sua rede)
BACKEND_URL=http://192.168.1.100:6660
```

Ou edite em `gradle.properties`:

```properties
backendUrl=http://192.168.1.100:6660
```

### 2. Instalação de Dependências

O projeto usa:

- **Retrofit**: Para requisições HTTP
- **OkHttp**: Cliente HTTP com interceptadores
- **Moshi**: Serialização JSON
- **Coroutines**: Programação assíncrona
- **Compose Material 3**: UI moderna

### 3. Compilar e Executar

```bash
# Via Android Studio
1. Abrir projeto em Android Studio
2. Aguardar sincronização Gradle
3. Conectar device ou iniciar emulador
4. Clicar em "Run" (Shift + F10)

# Via Gradle (terminal)
gradlew.bat build
gradlew.bat installDebug
```

## Estrutura do Projeto

```
mobile/
├── app/
│   ├── src/
│   │   ├── main/
│   │   │   ├── kotlin/com/chatbot/pim/
│   │   │   │   ├── MainActivity.kt
│   │   │   │   ├── ui/
│   │   │   │   │   ├── screens/
│   │   │   │   │   │   ├── LoginScreen.kt
│   │   │   │   │   │   ├── RegisterScreen.kt
│   │   │   │   │   │   └── ChatScreen.kt
│   │   │   │   │   └── theme/
│   │   │   │   │       ├── Theme.kt
│   │   │   │   │       ├── Color.kt
│   │   │   │   │       └── Type.kt
│   │   │   │   ├── viewmodel/
│   │   │   │   │   ├── AuthViewModel.kt
│   │   │   │   │   └── ChatViewModel.kt
│   │   │   │   ├── repository/
│   │   │   │   │   ├── AuthRepository.kt
│   │   │   │   │   └── ChatRepository.kt
│   │   │   │   ├── api/
│   │   │   │   │   ├── ApiClient.kt
│   │   │   │   │   ├── ApiService.kt
│   │   │   │   │   └── models/
│   │   │   │   │       ├── LoginRequest.kt
│   │   │   │   │       ├── LoginResponse.kt
│   │   │   │   │       ├── RegisterRequest.kt
│   │   │   │   │       ├── ChatRequest.kt
│   │   │   │   │       ├── ChatResponse.kt
│   │   │   │   │       └── ChatMessage.kt
│   │   │   │   └── util/
│   │   │   │       └── PreferencesManager.kt
│   │   │   ├── res/
│   │   │   │   ├── values/
│   │   │   │   │   ├── strings.xml
│   │   │   │   │   └── colors.xml
│   │   │   │   └── drawable/
│   │   │   │       └── ic_launcher.xml
│   │   │   └── AndroidManifest.xml
│   │   └── test/
│   ├── build.gradle.kts
│   └── proguard-rules.pro
├── build.gradle.kts
├── settings.gradle.kts
└── gradle.properties
```

## APIs Utilizadas

### Login

```
POST /api/users/login
Content-Type: application/json

{
  "email": "usuario@email.com",
  "password": "senha"
}

Response:
{
  "success": true,
  "message": "Login realizado com sucesso",
  "user": {
    "id": 1,
    "email": "usuario@email.com",
    "role": "user"
  }
}
```

### Registrar

```
POST /api/users/register
Content-Type: application/json

{
  "email": "novo@email.com",
  "password": "senha",
  "role": "user"
}

Response: Similar ao Login
```

### Chat

```
POST /api/chat
Content-Type: application/json

{
  "messages": [
    {
      "role": "user",
      "content": "Olá"
    }
  ]
}

Response:
{
  "success": true,
  "message": "Resposta do bot",
  "response": "..."
}
```

## Troubleshooting

### Conexão Recusada (Connection Refused)

- Verifique se o backend está rodando
- Confirme o IP e porta corretos
- Em emulador: use `10.0.2.2:6660` para localhost

### SSL/TLS Errors

- App usa HTTP (não recomendado para produção)
- Para HTTPS, adicione certificados ao projeto

### Permissões

O app requer:

- `INTERNET`: Para requisições HTTP
- `ACCESS_NETWORK_STATE`: Para verificar conectividade

Essas são adicionadas automaticamente no `AndroidManifest.xml`

## Notas de Desenvolvimento

- Todas as requisições são assíncronas (Coroutines)
- Token de sessão é armazenado em SharedPreferences
- UI é construída com Jetpack Compose
- Validações client-side básicas implementadas

## Contato

Para dúvidas ou problemas, consulte a documentação do Android Studio ou contate o time de desenvolvimento.
