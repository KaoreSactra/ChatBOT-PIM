# ChatBOT-PIM Mobile - Sumário de Arquivos

## Arquivos Criados

### 📁 Estrutura do Projeto

```
mobile/
├── README.md                          # Documentação principal
├── INSTALL.md                         # Guia de instalação
├── NETWORK_CONFIG.md                  # Configuração de rede
├── .gitignore                         # Ignorar arquivos no Git
├── gradle.properties                  # Configuração Gradle (URL BACKEND)
├── settings.gradle.kts                # Configuração multi-modulo
├── build.gradle.kts                   # Build script raiz
├── gradlew                            # Wrapper Gradle (Linux/Mac)
├── gradlew.bat                        # Wrapper Gradle (Windows)
│
├── gradle/wrapper/
│   └── gradle-wrapper.properties      # Versão Gradle
│
└── app/
    ├── build.gradle.kts               # Build script app
    ├── proguard-rules.pro             # Regras ProGuard/R8
    │
    └── src/main/
        ├── AndroidManifest.xml        # Manifest da app
        │
        ├── kotlin/com/chatbot/pim/
        │   ├── MainActivity.kt        # Activity principal
        │   │
        │   ├── api/
        │   │   ├── ApiClient.kt       # Cliente Retrofit (singleton)
        │   │   ├── ApiService.kt      # Interface de endpoints
        │   │   │
        │   │   └── models/
        │   │       ├── AuthModels.kt  # LoginRequest, RegisterRequest, etc
        │   │       └── ChatModels.kt  # ChatMessage, ChatRequest, ChatResponse
        │   │
        │   ├── repository/
        │   │   ├── AuthRepository.kt  # Lógica Login/Register
        │   │   └── ChatRepository.kt  # Lógica Chat
        │   │
        │   ├── viewmodel/
        │   │   ├── AuthViewModel.kt   # ViewModel de autenticação
        │   │   └── ChatViewModel.kt   # ViewModel de chat
        │   │
        │   └── ui/
        │       ├── screens/
        │       │   ├── AuthScreens.kt # Telas de Login e Registro
        │       │   └── ChatScreen.kt  # Tela de Chat
        │       │
        │       └── theme/
        │           └── Theme.kt       # Tema Material Design 3
        │
        └── res/
            └── values/
                ├── strings.xml        # Strings da app
                ├── colors.xml         # Paleta de cores
                └── themes.xml         # Temas XML
```

## Dependências Adicionadas

### build.gradle.kts (app)

```kotlin
// Android & Jetpack
androidx.core:core-ktx:1.12.0
androidx.lifecycle:lifecycle-runtime-ktx:2.6.2
androidx.activity:activity-compose:1.8.1

// Compose
androidx.compose.ui:ui
androidx.compose.material3:material3:1.1.2
androidx.compose.material:material-icons-extended
androidx.navigation:navigation-compose:2.7.5

// Retrofit + OkHttp
com.squareup.retrofit2:retrofit:2.9.0
com.squareup.retrofit2:converter-moshi:2.9.0
com.squareup.okhttp3:okhttp:4.11.0
com.squareup.okhttp3:logging-interceptor:4.11.0

// JSON
com.squareup.moshi:moshi:1.15.0
com.squareup.moshi:moshi-kotlin:1.15.0

// Coroutines
org.jetbrains.kotlinx:kotlinx-coroutines-android:1.7.3
org.jetbrains.kotlinx:kotlinx-coroutines-core:1.7.3

// Storage
androidx.datastore:datastore-preferences:1.0.0

// Permissions
com.google.accompanist:accompanist-permissions:0.33.2-alpha
```

## Configuração

### gradle.properties

Define a URL do backend. Ajuste conforme sua rede:

```properties
# Para Emulador
backendUrl=http://10.0.2.2:6660

# Para Device Real (encontre seu IP com ipconfig)
# backendUrl=http://192.168.1.100:6660
```

### AndroidManifest.xml

Permissões necessárias:

- `INTERNET` - Para requisições HTTP
- `ACCESS_NETWORK_STATE` - Para verificar conectividade
- `android:usesCleartextTraffic="true"` - Permite HTTP (não HTTPS)

## Fluxo da Aplicação

### 1. Autenticação (LoginScreen → RegisterScreen)

```
User Input (Email/Senha)
    ↓
AuthViewModel.login() / register()
    ↓
AuthRepository.login() / register()
    ↓
ApiService (Retrofit)
    ↓
Backend (/api/users/login ou /api/users/register)
    ↓
LoginResponse → ViewModel → UI atualizada
```

### 2. Chat (ChatScreen)

```
User Input (Mensagem)
    ↓
ChatViewModel.sendMessage()
    ↓
ChatRepository.sendMessage()
    ↓
ApiService (Retrofit)
    ↓
Backend (/api/chat)
    ↓
ChatResponse → ViewModel → UI atualizada
```

## Padrões Arquiteturais Utilizados

1. **MVVM** (Model-View-ViewModel)

   - ViewModel: Lógica de UI, estado compartilhado
   - Views: Composables Jetpack Compose
   - Models: Data classes

2. **Repository Pattern**

   - AuthRepository: Abstrai chamadas API de auth
   - ChatRepository: Abstrai chamadas API de chat

3. **Dependency Injection**

   - ApiClient singleton cria todos os serviços
   - ViewModels instanciados por viewModel()

4. **Coroutines**
   - Requisições assíncronas não bloqueantes
   - withContext(Dispatchers.IO) para operações de rede

## Endpoints da API Utilizados

### POST /api/users/login

- **Request:** `LoginRequest(email, password)`
- **Response:** `LoginResponse(success, message, user)`

### POST /api/users/register

- **Request:** `RegisterRequest(email, password, role)`
- **Response:** `LoginResponse(success, message, user)`

### POST /api/chat

- **Request:** `ChatRequest(messages: List<ChatMessage>)`
- **Response:** `ChatResponse(success, message, response)`

## Features Implementadas

✅ Login de usuários
✅ Registro de novos usuários
✅ Chat com histórico de mensagens
✅ UI moderno com Jetpack Compose
✅ Suporte a diferentes temas de cores
✅ Tratamento de erros com Snackbar
✅ Logging de requisições HTTP
✅ Suporte a emulador e device real
✅ Permissões de internet configuradas
✅ Configuração dinâmica de backend URL

## Próximos Passos (Opcional)

1. **Armazenar Token/Session**

   - Usar DataStore para guardar credenciais
   - Enviar token em Authorization header

2. **Validações Avançadas**

   - Validar email com regex
   - Força de senha (caracteres especiais, etc)

3. **Histórico Persistente**

   - Room Database para guardar mensagens localmente

4. **Compartilhamento**

   - Exportar chat como PDF/TXT

5. **Temas Customizáveis**

   - Tema claro/escuro
   - Customização de cores pelo usuário

6. **Notificações**

   - Novas mensagens com Firebase Cloud Messaging

7. **Internacionalização**
   - Suporte para múltiplos idiomas

## Troubleshooting Rápido

| Problema            | Solução                                          |
| ------------------- | ------------------------------------------------ |
| Connection refused  | Verificar se backend está rodando (`dotnet run`) |
| 404 Not Found       | Verificar URLs dos endpoints em ApiService.kt    |
| JSON Parse Error    | Verificar se resposta vem em camelCase           |
| Firewall bloqueando | Abrir porta 6660 no firewall                     |
| Build falha         | Executar `./gradlew clean`                       |
| Emulador lento      | Aumentar RAM/CPU em AVD Manager                  |

## Documentação Útil

- **Android Developers**: https://developer.android.com/
- **Jetpack Compose**: https://developer.android.com/jetpack/compose
- **Retrofit**: https://square.github.io/retrofit/
- **Coroutines**: https://kotlinlang.org/docs/coroutines-overview.html
- **Material Design 3**: https://m3.material.io/

## Arquivos Importantes para Editar

1. **Mudar Backend URL**: `gradle.properties` (linha 1)
2. **Adicionar Endpoints**: `api/ApiService.kt`
3. **Mudar Interface**: `ui/screens/*.kt`
4. **Adicionar Lógica**: `viewmodel/*.kt`

## Versionamento

- **Kotlin**: 1.9.21
- **Gradle**: 8.2
- **Android SDK**: 34 (compileSdk)
- **Min SDK**: 24 (Android 7.0)

---

**Criado em**: 24 de Novembro de 2025
**Versão**: 1.0
**Status**: Pronto para compilar e executar
