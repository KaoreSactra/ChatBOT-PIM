#!/bin/bash
# setup-and-build.sh - Script para setup automático do projeto mobile

set -e

echo "╔════════════════════════════════════════════════════════════╗"
echo "║         ChatBOT-PIM Mobile - Auto Setup Script             ║"
echo "╚════════════════════════════════════════════════════════════╝"
echo ""

# Cores
GREEN='\033[0;32m'
BLUE='\033[0;34m'
YELLOW='\033[1;33m'
NC='\033[0m'

# Função para imprimir seções
print_section() {
    echo ""
    echo -e "${BLUE}► $1${NC}"
    echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
}

# Função para imprimir sucesso
print_success() {
    echo -e "${GREEN}✓ $1${NC}"
}

# Função para imprimir aviso
print_warning() {
    echo -e "${YELLOW}⚠ $1${NC}"
}

# 1. Verificar pré-requisitos
print_section "Verificando Pré-requisitos"

# Verificar se está na pasta correta
if [ ! -f "settings.gradle.kts" ]; then
    echo "❌ Erro: Execute este script da raiz da pasta 'mobile'"
    echo "Exemplo: cd ChatBOT-PIM/mobile && bash setup-and-build.sh"
    exit 1
fi
print_success "Pasta do projeto detectada"

# Verificar Java
if ! command -v java &> /dev/null; then
    print_warning "Java não encontrado. Certifique-se que Android Studio está instalado"
    exit 1
fi
print_success "Java instalado"

# Verificar Gradle wrapper
if [ ! -f "gradlew" ]; then
    print_warning "gradlew não encontrado, baixando..."
    chmod +x gradlew
fi
print_success "Gradle wrapper disponível"

# 2. Limpeza
print_section "Limpando Build Anterior"
./gradlew clean
print_success "Build anterior removido"

# 3. Validar estrutura
print_section "Validando Estrutura do Projeto"
bash validate_structure.sh
print_success "Estrutura validada"

# 4. Download de dependências
print_section "Baixando Dependências"
./gradlew dependencies --refresh-dependencies > /dev/null 2>&1
print_success "Dependências baixadas"

# 5. Compilar
print_section "Compilando App"
./gradlew build -x test
print_success "App compilada com sucesso"

# 6. Informações finais
print_section "Setup Concluído"

echo ""
echo "✨ Seu projeto está pronto! ✨"
echo ""
echo "Próximos passos:"
echo ""
echo "  1️⃣  Abrir em Android Studio:"
echo "      open -a \"Android Studio\" ."
echo ""
echo "  2️⃣  Editar gradle.properties com URL do backend"
echo ""
echo "  3️⃣  Conectar device ou iniciar emulador"
echo ""
echo "  4️⃣  Instalar app:"
echo "      ./gradlew installDebug"
echo ""
echo "  5️⃣  Ou executar diretamente:"
echo "      ./gradlew installDebug && adb shell am start -n com.chatbot.pim/.MainActivity"
echo ""
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo ""
echo "📖 Documentação:"
echo "   - QUICK_START.md      → Comece aqui (5 passos)"
echo "   - INSTALL.md          → Instalação detalhada"
echo "   - NETWORK_CONFIG.md   → Conectar ao backend"
echo ""
echo "🐛 Problema? Verifique:"
echo "   - Backend rodando: netstat -ano | findstr :6660"
echo "   - URL em gradle.properties"
echo "   - Logcat: View > Tool Windows > Logcat"
echo ""
