#!/bin/bash
# Script para validar estrutura do projeto mobile

echo "================================"
echo "Validando Estrutura - ChatBOT-PIM Mobile"
echo "================================"
echo ""

# Cores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Array de arquivos obrigatórios
declare -a FILES=(
    "README.md"
    "INSTALL.md"
    "NETWORK_CONFIG.md"
    "FILE_STRUCTURE.md"
    ".gitignore"
    "gradle.properties"
    "settings.gradle.kts"
    "build.gradle.kts"
    "gradlew"
    "gradlew.bat"
    "gradle/wrapper/gradle-wrapper.properties"
    "app/build.gradle.kts"
    "app/proguard-rules.pro"
    "app/src/main/AndroidManifest.xml"
    "app/src/main/kotlin/com/chatbot/pim/MainActivity.kt"
    "app/src/main/kotlin/com/chatbot/pim/api/ApiClient.kt"
    "app/src/main/kotlin/com/chatbot/pim/api/ApiService.kt"
    "app/src/main/kotlin/com/chatbot/pim/api/models/AuthModels.kt"
    "app/src/main/kotlin/com/chatbot/pim/api/models/ChatModels.kt"
    "app/src/main/kotlin/com/chatbot/pim/repository/AuthRepository.kt"
    "app/src/main/kotlin/com/chatbot/pim/repository/ChatRepository.kt"
    "app/src/main/kotlin/com/chatbot/pim/viewmodel/AuthViewModel.kt"
    "app/src/main/kotlin/com/chatbot/pim/viewmodel/ChatViewModel.kt"
    "app/src/main/kotlin/com/chatbot/pim/ui/theme/Theme.kt"
    "app/src/main/kotlin/com/chatbot/pim/ui/screens/AuthScreens.kt"
    "app/src/main/kotlin/com/chatbot/pim/ui/screens/ChatScreen.kt"
    "app/src/main/res/values/strings.xml"
    "app/src/main/res/values/colors.xml"
    "app/src/main/res/values/themes.xml"
)

# Contar arquivos
TOTAL=${#FILES[@]}
FOUND=0
MISSING=0

echo "Verificando $TOTAL arquivos..."
echo ""

for file in "${FILES[@]}"; do
    if [ -f "$file" ]; then
        echo -e "${GREEN}✓${NC} $file"
        ((FOUND++))
    else
        echo -e "${RED}✗${NC} $file"
        ((MISSING++))
    fi
done

echo ""
echo "================================"
echo -e "Resultado: ${GREEN}$FOUND encontrados${NC}, ${RED}$MISSING faltando${NC}"
echo "================================"

if [ $MISSING -eq 0 ]; then
    echo -e "${GREEN}✓ Estrutura validada com sucesso!${NC}"
    echo ""
    echo "Próximos passos:"
    echo "1. Abrir em Android Studio"
    echo "2. Editar gradle.properties com a URL do backend"
    echo "3. Executar: ./gradlew build"
    echo "4. Executar app no emulador ou device"
    exit 0
else
    echo -e "${YELLOW}⚠ Arquivos faltando! Verifique a estrutura.${NC}"
    exit 1
fi
