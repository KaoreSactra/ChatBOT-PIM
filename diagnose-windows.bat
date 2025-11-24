@echo off
REM Script para diagnosticar problemas de NetworkError

echo.
echo ╔════════════════════════════════════════════════════════╗
echo ║     ChatBot - Network Error Diagnostics               ║
echo ╚════════════════════════════════════════════════════════╝
echo.

echo [1/5] Verificando se Backend está rodando na porta 6660...
netstat -ano | findstr :6660 >nul
if %errorlevel% equ 0 (
    echo [OK] Backend está rodando na porta 6660
) else (
    echo [ERRO] Nenhum processo na porta 6660
    echo Solução: Inicie o backend com: startup.bat (opção 1 ou 3)
)
echo.

echo [2/5] Verificando se Frontend está rodando na porta 6661...
netstat -ano | findstr :6661 >nul
if %errorlevel% equ 0 (
    echo [OK] Frontend está rodando na porta 6661
) else (
    echo [ERRO] Nenhum processo na porta 6661
    echo Solução: Inicie o frontend com: startup.bat (opção 2 ou 3)
)
echo.

echo [3/5] Tentando conectar ao Backend...
powershell -Command "try { $response = Invoke-WebRequest -Uri 'http://localhost:6660/health' -TimeoutSec 3 -ErrorAction Stop; Write-Host '[OK] Backend respondendo' } catch { Write-Host '[ERRO] Backend não responde'; Write-Host 'Erro:' $_.Exception.Message }"
echo.

echo [4/5] Verificando arquivo .env...
if exist ".env" (
    echo [OK] Arquivo .env encontrado
    echo Conteúdo:
    type .env | findstr /i "FRONTEND_API_BASE_URL"
) else (
    echo [ERRO] Arquivo .env não encontrado na raiz do projeto
    echo Solução: Execute startup.bat (opção 1, 2 ou 3) para criar .env automaticamente
)
echo.

echo [5/5] Verificando portas bloqueadas...
for /f "tokens=5" %%a in ('netstat -ano ^| findstr :6660') do (
    echo [INFO] Processo usando porta 6660: PID %%a
)
for /f "tokens=5" %%a in ('netstat -ano ^| findstr :6661') do (
    echo [INFO] Processo usando porta 6661: PID %%a
)
echo.

echo ═════════════════════════════════════════════════════════
echo Próximos passos:
echo.
echo 1. Se Backend/Frontend não estão rodando:
echo    Execute: startup.bat
echo    Escolha: 3 (Iniciar ambos)
echo.
echo 2. Se .env não existe:
echo    Execute: startup.bat
echo    Escolha: 3 (Iniciar ambos)
echo    Isso criará .env automaticamente
echo.
echo 3. Se Backend responde mas Frontend não:
echo    - Abra DevTools (F12) no navegador
echo    - Vá na aba Network
echo    - Tente fazer login novamente
echo    - Procure pela requisição vermelha (erro)
echo    - Clique nela para ver detalhes
echo.
echo 4. Se nada funcionar:
echo    Limpe tudo com: startup.bat (opção 5)
echo    Depois: startup.bat (opção 3)
echo ═════════════════════════════════════════════════════════
echo.

pause
