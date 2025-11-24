# ChatBOT-PIM 🤖

Sistema de chatbot inteligente para suporte técnico com integração ao Google Gemini, desenvolvido com C# .NET e Razor Pages.

## 📋 Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- **Git** - Para clonar o repositório
- **.NET SDK 8.0+** - [Download](https://dotnet.microsoft.com/download)
- **SQLite** (opcional) - Banco de dados local para desenvolvimento

### Verificar Instalações

```bash
# Verificar Git
git --version

# Verificar .NET SDK
dotnet --version
```

## 🚀 Instalação Rápida

### 1. Clonar o Repositório

```bash
git clone https://github.com/KaoreSactra/ChatBOT-PIM.git
cd ChatBOT-PIM
```

### 2. Restaurar Dependências

```bash
# Backend
cd backend
dotnet restore
cd ..

# Frontend
cd frontend/app
dotnet restore
cd ../..
```

### 3. Compilar Projetos

```bash
# Backend
cd backend
dotnet build
cd ..

# Frontend
cd frontend/app
dotnet build
cd ../..
```

## 🔐 Configuração de Variáveis de Ambiente

O projeto utiliza variáveis de ambiente para dados sensíveis. Você precisa criar arquivos `.env` em dois locais:

### Criar `.env` na Raiz do Projeto

```bash
cat > .env << 'EOF'
# Google Gemini API
GOOGLE_GEMINI_API_KEY=sua_chave_api_aqui

# Backend Server
BACKEND_URL=http://localhost:6660
BACKEND_PORT=6660

# Frontend Server
FRONTEND_API_BASE_URL=http://localhost:6660
EOF
```

### Criar `.env` no Backend

```bash
cat > backend/.env << 'EOF'
# Google Gemini API
GOOGLE_GEMINI_API_KEY=sua_chave_api_aqui

# Backend Server
BACKEND_URL=http://localhost:6660
BACKEND_PORT=6660

# Frontend Server
FRONTEND_API_BASE_URL=http://localhost:6660
EOF
```

### Criar `.env` no Frontend

```bash
cat > frontend/app/.env << 'EOF'
# Google Gemini API
GOOGLE_GEMINI_API_KEY=sua_chave_api_aqui

# Backend Server
BACKEND_URL=http://localhost:6660
BACKEND_PORT=6660

# Frontend Server
FRONTEND_API_BASE_URL=http://localhost:6660
EOF
```

**⚠️ Importante:** Substitua `sua_chave_api_aqui` pela sua chave real da API Google Gemini. [Obter chave](https://makersuite.google.com/app/apikey)

## ▶️ Executar a Aplicação

### ⭐ Opção 1: Script Startup (Recomendado para Todos)

Use o script automatizado que funciona em **Windows, Linux e macOS**:

#### No Windows:

Clique duas vezes em `startup.bat` ou execute no prompt:

```cmd
startup.bat
```

#### No Linux/macOS:

```bash
chmod +x startup.sh
./startup.sh
```

O script irá:

- ✅ Detectar automaticamente o diretório do projeto
- ✅ Criar arquivos `.env` com valores padrão (se não existirem)
- ✅ Oferecer menu para iniciar, compilar ou limpar projetos
- ✅ Funcionar em qualquer PC sem precisar de configurações extras

**Menu de Opções:**

1. Iniciar apenas API Backend
2. Iniciar apenas Web Frontend
3. Iniciar ambos os projetos
4. Compilar ambos os projetos
5. Limpar e reinstalar dependências
6. Parar todos os processos

### Opção 2: Executar em Dois Terminais

**Terminal 1 - Backend:**

```bash
cd backend
dotnet run
```

Você verá uma mensagem como:

```
Now listening on: http://0.0.0.0:6660
```

**Terminal 2 - Frontend:**

```bash
cd frontend/app
dotnet run
```

Você verá uma mensagem como:

```
Now listening on: http://0.0.0.0:6661
```

### Opção 3: Script Automatizado Manual

```bash
#!/bin/bash

# Limpar builds antigos
cd backend && dotnet clean && rm -rf bin obj && cd ..
cd frontend/app && dotnet clean && rm -rf bin obj && cd ../..

# Iniciar Backend em background
cd backend && dotnet run &
BACKEND_PID=$!

# Aguardar backend iniciar
sleep 3

# Iniciar Frontend em background
cd frontend/app && dotnet run &
FRONTEND_PID=$!

echo "✅ Backend PID: $BACKEND_PID"
echo "✅ Frontend PID: $FRONTEND_PID"
echo ""
echo "🌐 Frontend disponível em: http://localhost:6661"
```

## ⚠️ Solução de Problemas

### No Windows: Erro "O sistema não pode encontrar o caminho especificado"

Se receber este erro ao tentar executar `startup.sh`, use `startup.bat` em vez disso:

```cmd
startup.bat
```

O arquivo `.bat` é o correto para Windows e não requer Git Bash ou WSL.

### No Windows: Erro "Script disabled"

Se receber erro de permissão, tente usar PowerShell com permissões de administrador:

```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

### No Linux/macOS: Erro de Permissão

Se receber "Permission denied", dê permissão de execução:

```bash
chmod +x startup.sh
```

echo "🔧 Backend disponível em: http://localhost:6660"
echo ""
echo "Para parar a aplicação, execute:"
echo "kill $BACKEND_PID $FRONTEND_PID"

````

Salve como `start.sh` e execute:

```bash
chmod +x start.sh
./start.sh
````

## 🌐 Acessar a Aplicação

Após iniciar ambos os servidores:

- **Frontend**: [http://localhost:6661](http://localhost:6661)
- **Backend API**: [http://localhost:6660](http://localhost:6660)

## 👤 Credenciais Padrão

O sistema cria automaticamente um usuário admin no primeiro acesso:

- **Email**: `admin@admin.com`
- **Senha**: `admin`

## 📁 Estrutura do Projeto

```
ChatBOT-PIM/
├── backend/                    # API em C# .NET
│   ├── Controllers/            # Endpoints da API
│   ├── Models/                 # Modelos de dados
│   ├── Data/                   # Contexto do banco de dados
│   ├── Program.cs              # Configuração da aplicação
│   ├── api-back.csproj         # Dependências do backend
│   └── .env                    # Variáveis de ambiente (não versionado)
│
├── frontend/
│   └── app/                    # Aplicação web com Razor Pages
│       ├── Pages/              # Páginas Razor
│       ├── Services/           # Serviços de integração com API
│       ├── Program.cs          # Configuração da aplicação
│       ├── app.csproj          # Dependências do frontend
│       └── .env                # Variáveis de ambiente (não versionado)
│
├── .env                        # Variáveis globais (não versionado)
├── .gitignore                  # Arquivos ignorados pelo Git
└── README.md                   # Este arquivo
```

## 🔧 Variáveis de Ambiente

As seguintes variáveis podem ser configuradas no `.env`:

| Variável                | Descrição                  | Exemplo                 |
| ----------------------- | -------------------------- | ----------------------- |
| `GOOGLE_GEMINI_API_KEY` | Chave da API Google Gemini | `AIzaSy...`             |
| `BACKEND_URL`           | URL do backend             | `http://localhost:6660` |
| `BACKEND_PORT`          | Porta do backend           | `6660`                  |
| `FRONTEND_API_BASE_URL` | URL da API para o frontend | `http://localhost:6660` |

## 🛠️ Comandos Úteis

### Limpar Builds

```bash
# Backend
cd backend && dotnet clean && rm -rf bin obj && cd ..

# Frontend
cd frontend/app && dotnet clean && rm -rf bin obj && cd ../..
```

### Rebuild Completo

```bash
# Backend
cd backend && dotnet clean && rm -rf bin obj && dotnet build && cd ..

# Frontend
cd frontend/app && dotnet clean && rm -rf bin obj && dotnet build && cd ../..
```

### Restaurar Dependências

```bash
# Backend
cd backend && dotnet restore && cd ..

# Frontend
cd frontend/app && dotnet restore && cd ../..
```

### Parar a Aplicação

Se a aplicação estiver rodando em background:

```bash
# Parar todos os processos dotnet run
pkill -f "dotnet run"

# Ou para um PID específico
kill <PID>
```

### Testar Conexão com Backend

```bash
# Via curl
curl -s http://localhost:6660/api/users | jq

# Via wget
wget -qO- http://localhost:6660/api/users
```

## 🔐 Segurança

- **Variáveis sensíveis** são armazenadas em `.env` e **não são versionadas** (protegidas pelo `.gitignore`)
- Dados de autenticação são protegidos com **BCrypt**
- As chaves da API nunca aparecem no repositório
- Arquivo `.gitignore` protege automaticamente:
  - `.env` e variações (`.env.local`, `.env.*.local`)
  - `appsettings.json` e `appsettings.Development.json`
  - Diretórios `bin/` e `obj/`
  - Arquivos de IDE (`.vs/`, `.vscode/`)

## 🐛 Troubleshooting

### Porta Já Está em Uso

Se receber erro `Address already in use`:

```bash
# Parar processos dotnet
pkill -f "dotnet run"

# Ou especificar portas diferentes no .env
BACKEND_PORT=6670
FRONTEND_API_BASE_URL=http://localhost:6670
```

### Erro ao Carregar `.env`

Certifique-se de que o arquivo `.env` existe no diretório correto:

```bash
# Verificar arquivos
ls -la .env
ls -la backend/.env
ls -la frontend/app/.env
```

### Erro de Conexão Backend

Verifique se o backend está rodando:

```bash
# Testar conectividade
curl http://localhost:6660

# Verificar processos
ps aux | grep "dotnet run"
```

### Erro de API Key Inválida

Certifique-se de que a chave do Google Gemini está corretamente configurada:

```bash
# Verificar se a chave está carregada
echo $GOOGLE_GEMINI_API_KEY
```

## 📚 Endpoints Principais da API

| Método | Endpoint              | Descrição                  |
| ------ | --------------------- | -------------------------- |
| POST   | `/api/users/login`    | Fazer login                |
| POST   | `/api/users/register` | Criar nova conta           |
| GET    | `/api/users`          | Listar usuários (admin)    |
| POST   | `/api/chat/send`      | Enviar mensagem ao chatbot |

## 🤝 Contribuindo

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo LICENSE para mais detalhes.

## 📧 Suporte

Para suporte, entre em contato através da página de issues do repositório:
[GitHub Issues](https://github.com/KaoreSactra/ChatBOT-PIM/issues)

---

**Desenvolvido com ❤️ usando C# .NET e Razor Pages**
