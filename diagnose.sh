#!/bin/bash
# Script para diagnosticar problemas de NetworkError no Linux/macOS

echo ""
echo "╔════════════════════════════════════════════════════════╗"
echo "║     ChatBot - Network Error Diagnostics               ║"
echo "╚════════════════════════════════════════════════════════╝"
echo ""

echo "[1/5] Verificando se Backend está rodando na porta 6660..."
if lsof -i :6660 > /dev/null 2>&1; then
    echo "[OK] Backend está rodando na porta 6660"
    lsof -i :6660
else
    echo "[ERRO] Nenhum processo na porta 6660"
    echo "Solução: Inicie o backend com: ./startup.sh (opção 1 ou 3)"
fi
echo ""

echo "[2/5] Verificando se Frontend está rodando na porta 6661..."
if lsof -i :6661 > /dev/null 2>&1; then
    echo "[OK] Frontend está rodando na porta 6661"
    lsof -i :6661
else
    echo "[ERRO] Nenhum processo na porta 6661"
    echo "Solução: Inicie o frontend com: ./startup.sh (opção 2 ou 3)"
fi
echo ""

echo "[3/5] Tentando conectar ao Backend..."
if command -v curl > /dev/null 2>&1; then
    if curl -s http://localhost:6660/health > /dev/null 2>&1; then
        echo "[OK] Backend respondendo"
        curl -s http://localhost:6660/health | jq . 2>/dev/null || curl -s http://localhost:6660/health
    else
        echo "[ERRO] Backend não responde"
        echo "Tente: curl -v http://localhost:6660/health"
    fi
else
    echo "[INFO] curl não instalado. Teste manualmente:"
    echo "curl http://localhost:6660/health"
fi
echo ""

echo "[4/5] Verificando arquivo .env..."
if [ -f ".env" ]; then
    echo "[OK] Arquivo .env encontrado"
    echo "Conteúdo relevante:"
    grep "FRONTEND_API_BASE_URL\|BACKEND_PORT\|FRONTEND_PORT" .env || cat .env
else
    echo "[ERRO] Arquivo .env não encontrado na raiz do projeto"
    echo "Solução: Execute ./startup.sh (opção 1, 2 ou 3) para criar .env automaticamente"
fi
echo ""

echo "[5/5] Verificando processos dotnet..."
echo "Processos dotnet em execução:"
ps aux | grep "dotnet run" | grep -v grep || echo "Nenhum processo dotnet encontrado"
echo ""

echo "═════════════════════════════════════════════════════════"
echo "Próximos passos:"
echo ""
echo "1. Se Backend/Frontend não estão rodando:"
echo "   Execute: ./startup.sh"
echo "   Escolha: 3 (Iniciar ambos)"
echo ""
echo "2. Se .env não existe:"
echo "   Execute: ./startup.sh"
echo "   Escolha: 3 (Iniciar ambos)"
echo "   Isso criará .env automaticamente"
echo ""
echo "3. Se Backend responde mas Frontend não:"
echo "   - Abra DevTools (F12) no navegador"
echo "   - Vá na aba Network"
echo "   - Tente fazer login novamente"
echo "   - Procure pela requisição vermelha (erro)"
echo "   - Clique nela para ver detalhes"
echo ""
echo "4. Se nada funcionar:"
echo "   Limpe tudo com: ./startup.sh (opção 5)"
echo "   Depois: ./startup.sh (opção 3)"
echo "═════════════════════════════════════════════════════════"
echo ""
