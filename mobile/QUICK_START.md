# 🚀 Quick Start - ChatBOT-PIM Mobile

## 5 Passos para Começar

### 1️⃣ Abrir em Android Studio

```bash
# Opção A: Via terminal
android-studio mobile/

# Opção B: Manual
# 1. Abrir Android Studio
# 2. File → Open
# 3. Selecionar pasta "mobile"
```

### 2️⃣ Encontrar IP do Backend

**Seu PC está rodando o backend em qual IP?**

```bash
# Windows (PowerShell)
ipconfig | findstr "IPv4"

# Linux/Mac
ifconfig | grep "inet "
```

Exemplo: `192.168.1.100`

### 3️⃣ Editar gradle.properties

Abrir arquivo: `mobile/gradle.properties`

```properties
# Se usa EMULADOR (padrão):
backendUrl=http://10.0.2.2:6660

# Se usa DEVICE REAL, substitua pelo seu IP:
backendUrl=http://192.168.1.100:6660
```

**Salvar (Ctrl+S)**

### 4️⃣ Compilar e Executar

```bash
# Terminal dentro do Android Studio (Alt+F12)
./gradlew clean build

# Ou clique em: Build → Build Bundle(s) / APK(s)
```

### 5️⃣ Rodar no Emulador ou Device

```bash
# Conectar device USB ou iniciar emulador
./gradlew installDebug

# Ou via Android Studio:
# Run → Run 'app' (Shift + F10)
```

---

## ✅ Checklist Pré-Execução

- [ ] Backend rodando: `dotnet run` (porta 6660)
- [ ] IP do backend encontrado (ipconfig)
- [ ] gradle.properties editado com URL correta
- [ ] Android SDK 24+ instalado
- [ ] Java 17+ instalado (vem com Android Studio)
- [ ] Emulador/Device conectado

---

## 🐛 Problema? Veja Aqui

### ❌ "Connection refused"

- Backend não está rodando
- Verifique: `netstat -ano | findstr :6660`

### ❌ "Connection timed out"

- IP errado em gradle.properties
- Firewall bloqueando porta 6660
- Adicione em PowerShell (Admin):
  ```powershell
  netsh advfirewall firewall add rule name="Backend" dir=in action=allow protocol=tcp localport=6660
  ```

### ❌ "Gradle sync falha"

```bash
./gradlew clean
./gradlew sync
```

### ❌ "App fecha ao abrir"

- Veja logs: View → Tool Windows → Logcat
- Filtro: `OkHttp` ou `error`
- Verifique URL em gradle.properties

---

## 📊 Features Implementadas

✅ Login/Registro de usuários
✅ Chat em tempo real com IA
✅ Histórico de mensagens
✅ Interface moderna (Jetpack Compose)
✅ Comunicação com API backend
✅ Tratamento de erros
✅ Suporte a redes locais

---

## 📱 Primeira Execução

Quando abrir o app:

1. **Tela de Login**

   - Email: `admin@admin.com`
   - Senha: `admin`
   - Clique em "Entrar"

2. **Ou Registrar Nova Conta**

   - Email: seu email
   - Senha: mínimo 6 caracteres
   - Confirmar senha
   - Clique em "Registrar"

3. **Chat**
   - Digite uma pergunta relacionada a hardware/software
   - Aguarde resposta da IA
   - Histórico mantido durante a sessão

---

## 📂 Documentação Completa

- **README.md** - Visão geral e requisitos
- **INSTALL.md** - Instalação detalhada
- **NETWORK_CONFIG.md** - Configuração de rede
- **FILE_STRUCTURE.md** - Estrutura de arquivos
- **este arquivo** - Quick start

---

## 🔑 Credenciais Padrão

Usuário admin criado automaticamente:

```
Email: admin@admin.com
Senha: admin
Role: admin
```

---

## 🛠 Tecnologias

| Ferramenta     | Versão              |
| -------------- | ------------------- |
| Android Studio | Jellyfish 2023.3.1+ |
| Kotlin         | 1.9.21              |
| Android SDK    | 24+                 |
| Gradle         | 8.2                 |
| Compose        | Material 3          |

---

## 💡 Dicas

1. **Desenvolvimento Local**: Use `10.0.2.2:6660` para emulador
2. **Production**: Substitua IP e use HTTPS
3. **Logs**: Monitore em Logcat (View → Tool Windows → Logcat)
4. **Performance**: Device real é 10x mais rápido que emulador

---

## 📞 Suporte

Dúvidas?

1. Verifique Logcat para erros
2. Consulte NETWORK_CONFIG.md
3. Veja INSTALL.md para troubleshooting
4. Leia comentários no código

---

**Tudo pronto? Execute e divirta-se! 🎉**
