# 🤖 ChatBot PIM - Tutorial Completo

Guia passo a passo para instalar e executar o ChatBot PIM no Windows.

## Requisitos Obrigatórios

### 1. .NET 8.0 SDK

- Acesse: https://dotnet.microsoft.com/download/dotnet/8.0
- Clique em Download .NET SDK para Windows
- Execute o instalador e siga as instruções
- Reinicie o computador

**Verificar instalacao:**
```
dotnet --version
```

Voce deve ver: 8.0.xxx

### 2. Git (Opcional)

- Acesse: https://git-scm.com/download/win
- Execute o instalador
- Reinicie o computador

## Como Comecar

### Opcao A: Clonar com Git

```
cd C:\Users\SeuUsuario\Documents
git clone https://github.com/KaoreSactra/ChatBOT-PIM.git
cd ChatBOT-PIM
```

### Opcao B: Download Manual

1. Visite https://github.com/KaoreSactra/ChatBOT-PIM
2. Clique em Code → Download ZIP
3. Extraia em uma pasta
4. Abra a pasta no Prompt de Comando

## Primeira Execucao

### Opcao Mais Facil: Use o startup.bat

1. Abra a pasta ChatBOT-PIM
2. Clique 2 vezes em startup.bat
3. Digite 1 e pressione ENTER
4. Aguarde 10-15 segundos

Voce vera 3 janelas abrindo:
- Janela 1: Backend API (porta 6660)
- Janela 2: Frontend Web (porta 6661)
- Janela 3: Aplicacao Desktop

## Testando a Aplicacao

### Backend (API)

Abra seu navegador em:
http://localhost:6660/swagger

Voce vera uma documentacao interativa da API.

### Frontend (Web)

Acesse em seu navegador:
http://localhost:6661

Credenciais padrao:
- Email: admin@admin.com
- Senha: admin

Clique em Login e sera redirecionado para o chat.

### Desktop

A aplicacao abre automaticamente.
Faca login com as mesmas credenciais.

### Teste o Chat

No web ou desktop, clique na aba Chat e digite uma mensagem:
- Exemplo: "Qual é o melhor processador?"

Respostas esperadas:
- Com Google Gemini: Respostas inteligentes de IA
- Sem chave Gemini: Respostas genericas padrao

## Menu do startup.bat

Opcao 1: Iniciar tudo (Backend, Frontend e Desktop)
Opcao 2: Opcoes avancadas (iniciar apenas um, compilar, limpar, etc)
Opcao 3: Parar todos os processos

## Resolucao de Problemas

### "dotnet nao é reconhecido"

- Reinstale o .NET 8.0 SDK
- Reinicie o computador
- Abra uma nova janela de Prompt de Comando

### "Porta ja está em uso"

- No startup.bat, clique opcao 3 para parar tudo
- Aguarde 10 segundos
- Execute novamente com opcao 1

### "Erro ao conectar com API"

No Desktop:
- Verifique se o Backend está rodando (janela 1 aberta)
- Se nao, reinicie

No Web:
- Aguarde o backend iniciar (leva ~10 segundos)
- Se falhar, recarregue a página (F5)

### "Chat nao responde"

Causas:
1. Voce nao tem a chave do Google Gemini (normal, usará respostas padrao)
2. Conexao com internet com problema
3. Backend nao respondendo (reinicie com startup.bat)

### "Caracteres estranhos no startup.bat"

Se vir símbolos esquisitos:
- Use PowerShell em vez do Prompt de Comando
- Ou clique em Opcoes Avancadas em vez de digitar direto

## Estrutura do Projeto

ChatBOT-PIM/
├── backend/          (API do servidor)
├── frontend/         (Site/Aplicacao Web)
├── desktop/          (Aplicacao Desktop)
├── startup.bat       (Script para rodar tudo)
├── .env              (Configuracoes)
└── README.md         (Este arquivo)

## URLs e Credenciais Padrao

URLs:
- Frontend Web: http://localhost:6661
- Backend API: http://localhost:6660
- API Docs: http://localhost:6660/swagger

Credenciais:
- Email: admin@admin.com
- Senha: admin

## Recursos

Frontend Web:
- Login e Registro
- Chat com IA
- Dashboard administrativo
- Interface responsiva

Desktop:
- Aplicacao Windows nativa
- Login e Registro
- Chat em tempo real

Backend API:
- Autenticacao de usuarios
- Chat com Google Gemini
- Banco de dados em memoria
- Documentacao Swagger
- CORS habilitado

## Proximos Passos

1. Criar novo usuario: No frontend/desktop, clique em Registrar
2. Adicionar chave Gemini: https://ai.google.dev/
3. Explorar Dashboard: Se logar como admin
4. Testar API: http://localhost:6660/swagger

## Suporte

- GitHub: https://github.com/KaoreSactra/ChatBOT-PIM
- Issues: https://github.com/KaoreSactra/ChatBOT-PIM/issues

## Licenca

Este projeto é fornecido como está para fins educacionais e de demonstracao.

Ultima atualizacao: Novembro 2025
Versao: 1.0.0
