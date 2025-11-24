#!/bin/bash

# Script para executar o projeto web-front

echo "================================"
echo "ChatBot Web Frontend - ASP.NET"
echo "================================"
echo ""

# Verificar se está na pasta correta
if [ ! -f "web-front.csproj" ]; then
    echo "❌ Erro: Execute este script da pasta /web-front/web-front"
    exit 1
fi

# Menu
echo "Selecione uma opção:"
echo "1. Compilar (Build)"
echo "2. Executar (Run)"
echo "3. Compilar e Executar"
echo "4. Limpar (Clean)"
echo "5. Restaurar pacotes (Restore)"
echo ""
read -p "Opção (1-5): " opcao

case $opcao in
    1)
        echo ""
        echo "🔨 Compilando projeto..."
        dotnet build
        ;;
    2)
        echo ""
        echo "🚀 Executando projeto..."
        echo "Abra: http://localhost:5001"
        dotnet run
        ;;
    3)
        echo ""
        echo "🔨 Compilando projeto..."
        dotnet build
        echo ""
        echo "🚀 Executando projeto..."
        echo "Abra: http://localhost:5001"
        dotnet run
        ;;
    4)
        echo ""
        echo "🧹 Limpando projeto..."
        dotnet clean
        ;;
    5)
        echo ""
        echo "📦 Restaurando pacotes..."
        dotnet restore
        ;;
    *)
        echo "❌ Opção inválida"
        exit 1
        ;;
esac

echo ""
echo "✅ Concluído!"
