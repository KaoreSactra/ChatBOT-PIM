#!/bin/bash
# Script para iniciar ambos os projetos - API Backend e Web Frontend

echo "╔════════════════════════════════════════════════════════╗"
echo "║         ChatBot - Startup Script                       ║"
echo "║  Inicia API Backend e Web Frontend                    ║"
echo "╚════════════════════════════════════════════════════════╝"
echo ""

# Cores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Diretório base
BASE_DIR="/home/gustavoaragao/projects"

# Função para criar .env se não existir
setup_env() {
    local env_file="$1"
    local env_dir=$(dirname "$env_file")
    
    if [ ! -f "$env_file" ]; then
        echo -e "${YELLOW}⚠️  Criando $env_file...${NC}"
        cat > "$env_file" << 'EOF'
# Google Gemini API
GOOGLE_GEMINI_API_KEY=AIzaSyCu6kOrIl18cvcCmWAkpiKqkcsRVdXnNBs

# Backend Server
BACKEND_URL=http://192.168.1.113:6660
BACKEND_PORT=6660

# Frontend Server
FRONTEND_URL=http://192.168.1.113:6661
FRONTEND_PORT=6661
EOF
        echo -e "${GREEN}✓ $env_file criado${NC}"
    fi
}

# Carregar variáveis de ambiente
load_env() {
    if [ -f "$BASE_DIR/.env" ]; then
        export $(cat "$BASE_DIR/.env" | grep -v '^#' | xargs)
    fi
}

# Verifica se está no diretório correto
if [ ! -d "$BASE_DIR" ]; then
    echo -e "${RED}❌ Erro: Diretório de projetos não encontrado${NC}"
    exit 1
fi

cd "$BASE_DIR"

echo -e "${BLUE}📋 Verificando configuração...${NC}"
echo ""

# Verificar e criar .env files
setup_env "$BASE_DIR/.env"
setup_env "$BASE_DIR/backend/.env"
setup_env "$BASE_DIR/frontend/app/.env"

# Carregar variáveis de ambiente
load_env

echo ""
echo -e "${BLUE}📋 Verificando projetos...${NC}"
echo ""

# Verificar API Backend
if [ -d "$BASE_DIR/backend" ]; then
    echo -e "${GREEN}✓ API Backend encontrada${NC}"
else
    echo -e "${RED}✗ API Backend não encontrada${NC}"
fi

# Verificar Web Frontend
if [ -d "$BASE_DIR/frontend/app" ]; then
    echo -e "${GREEN}✓ Web Frontend encontrada${NC}"
else
    echo -e "${RED}✗ Web Frontend não encontrada${NC}"
fi

echo ""
echo -e "${YELLOW}URLs Configuradas:${NC}"
echo -e "  Backend:  ${BLUE}${BACKEND_URL}${NC}"
echo -e "  Frontend: ${BLUE}${FRONTEND_URL}${NC}"
echo ""
echo -e "${YELLOW}Escolha uma opção:${NC}"
echo "1. Iniciar apenas API Backend"
echo "2. Iniciar apenas Web Frontend"
echo "3. Iniciar ambos os projetos"
echo "4. Compilar ambos os projetos"
echo "5. Limpar e reinstalar dependências"
echo "6. Parar todos os processos"
echo ""
read -p "Opção (1-6): " opcao

case $opcao in
    1)
        echo ""
        echo -e "${BLUE}🚀 Iniciando API Backend...${NC}"
        cd "$BASE_DIR/backend"
        echo -e "${YELLOW}API Backend rodará em: http://localhost:6660${NC}"
        dotnet run
        ;;
    2)
        echo ""
        echo -e "${BLUE}🚀 Iniciando Web Frontend...${NC}"
        cd "$BASE_DIR/frontend/app"
        echo -e "${YELLOW}Web Frontend rodará em: http://localhost:6661${NC}"
        dotnet run
        ;;
    3)
        echo ""
        echo -e "${BLUE}🚀 Iniciando ambos os projetos...${NC}"
        echo ""
        
        # Verificar se tmux está disponível
        if command -v tmux &> /dev/null; then
            # Usar tmux se disponível
            echo -e "${GREEN}📌 Usando tmux para sessão...${NC}"
            
            # Verificar se sessão já existe
            if tmux has-session -t chatbot 2>/dev/null; then
                echo -e "${YELLOW}Sessão 'chatbot' já existe. Encerrando...${NC}"
                tmux kill-session -t chatbot
                sleep 1
            fi
            
            # Criar sessão tmux com janela backend
            tmux new-session -d -s chatbot -n backend
            tmux send-keys -t chatbot:backend "cd $BASE_DIR/backend && export ASPNETCORE_URLS='http://0.0.0.0:${BACKEND_PORT}' && echo '🚀 Iniciando API Backend em ${BACKEND_URL}' && dotnet run" Enter
            
            sleep 3
            
            # Janela 2 - Frontend
            tmux new-window -t chatbot -n frontend
            tmux send-keys -t chatbot:frontend "cd $BASE_DIR/frontend/app && export ASPNETCORE_URLS='http://0.0.0.0:${FRONTEND_PORT}' && echo '🚀 Iniciando Web Frontend em ${FRONTEND_URL}' && dotnet run" Enter
            
            echo ""
            echo -e "${GREEN}✅ Ambos os projetos foram iniciados em tmux!${NC}"
            echo ""
            echo -e "${YELLOW}Para conectar à sessão tmux:${NC}"
            echo -e "  ${BLUE}tmux attach-session -t chatbot${NC}"
            echo ""
            echo -e "${YELLOW}Para alternar entre janelas:${NC}"
            echo -e "  ${BLUE}Ctrl+B e depois N (próxima) ou P (anterior)${NC}"
            echo ""
            
        else
            # Fallback para execução em background sem tmux
            echo -e "${GREEN}📌 Iniciando em background...${NC}"
            
            cd "$BASE_DIR/backend"
            echo '🚀 Iniciando API Backend em '${BACKEND_URL}
            nohup dotnet run > /tmp/backend.log 2>&1 &
            BACKEND_PID=$!
            echo -e "${GREEN}Backend PID: $BACKEND_PID${NC}"
            
            sleep 3
            
            cd "$BASE_DIR/frontend/app"
            echo '🚀 Iniciando Web Frontend em '${FRONTEND_URL}
            nohup dotnet run > /tmp/frontend.log 2>&1 &
            FRONTEND_PID=$!
            echo -e "${GREEN}Frontend PID: $FRONTEND_PID${NC}"
            
            echo ""
        fi
        
        echo -e "${BLUE}📍 URLs:${NC}"
        echo -e "   API Backend:   ${YELLOW}${BACKEND_URL}${NC}"
        echo -e "   Web Frontend:  ${YELLOW}${FRONTEND_URL}${NC}"
        echo -e "   Swagger API:   ${YELLOW}${BACKEND_URL}/swagger${NC}"
        echo ""
        echo -e "${YELLOW}⏳ Aguarde alguns segundos para inicialização...${NC}"
        ;;
    4)
        echo ""
        echo -e "${BLUE}🔨 Compilando API Backend...${NC}"
        cd "$BASE_DIR/backend"
        dotnet build
        
        echo ""
        echo -e "${BLUE}🔨 Compilando Web Frontend...${NC}"
        cd "$BASE_DIR/frontend/app"
        dotnet build
        
        echo ""
        echo -e "${GREEN}✅ Compilação concluída!${NC}"
        ;;
    5)
        echo ""
        echo -e "${BLUE}🧹 Limpando e reinstalando...${NC}"
        
        echo -e "${YELLOW}Limpando API Backend...${NC}"
        cd "$BASE_DIR/backend"
        dotnet clean
        rm -rf bin obj
        
        echo -e "${YELLOW}Limpando Web Frontend...${NC}"
        cd "$BASE_DIR/frontend/app"
        dotnet clean
        rm -rf bin obj
        
        echo ""
        echo -e "${BLUE}📦 Restaurando pacotes...${NC}"
        
        echo -e "${YELLOW}Restaurando API Backend...${NC}"
        cd "$BASE_DIR/backend"
        dotnet restore
        
        echo -e "${YELLOW}Restaurando Web Frontend...${NC}"
        cd "$BASE_DIR/frontend/app"
        dotnet restore
        
        echo ""
        echo -e "${GREEN}✅ Limpeza e reinstalação concluídas!${NC}"
        ;;
    6)
        echo ""
        echo -e "${BLUE}🛑 Parando todos os processos...${NC}"
        pkill -f "dotnet run"
        echo -e "${GREEN}✓ Processos parados${NC}"
        ;;
    *)
        echo -e "${RED}❌ Opção inválida${NC}"
        exit 1
        ;;
esac

echo ""
echo -e "${GREEN}✅ Pronto!${NC}"
