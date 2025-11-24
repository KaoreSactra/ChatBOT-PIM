@echo off
REM Script para iniciar ambos os projetos - API Backend e Web Frontend (Windows)

setlocal enabledelayedexpansion

echo.
echo ╔════════════════════════════════════════════════════════╗
echo ║         ChatBot - Startup Script (Windows)             ║
echo ║  Inicia API Backend e Web Frontend                    ║
echo ╚════════════════════════════════════════════════════════╝
echo.

REM Detectar diretório onde o script está localizado
set "BASE_DIR=%~dp0"
REM Remover barra final se existir
if "%BASE_DIR:~-1%"=="\" set "BASE_DIR=%BASE_DIR:~0,-1%"

REM Portas padrão
set "BACKEND_PORT=6660"
set "FRONTEND_PORT=6661"
set "BACKEND_URL=http://localhost:%BACKEND_PORT%"
set "FRONTEND_URL=http://localhost:%FRONTEND_PORT%"

echo [INFO] Diretório raiz: %BASE_DIR%
echo.

REM Verificar se os diretórios existem
if not exist "%BASE_DIR%\backend" (
    echo [ERRO] Diretório backend não encontrado em: %BASE_DIR%\backend
    pause
    exit /b 1
)

if not exist "%BASE_DIR%\frontend" (
    echo [ERRO] Diretório frontend não encontrado em: %BASE_DIR%\frontend
    pause
    exit /b 1
)

echo [OK] API Backend encontrada
echo [OK] Web Frontend encontrada
echo.
echo URLs Configuradas:
echo   Backend:  %BACKEND_URL%
echo   Frontend: %FRONTEND_URL%
echo.
echo Escolha uma opcao:
echo 1. Iniciar apenas API Backend
echo 2. Iniciar apenas Web Frontend
echo 3. Iniciar ambos os projetos
echo 4. Compilar ambos os projetos
echo 5. Limpar e reinstalar dependencias
echo 6. Parar todos os processos
echo.

set /p opcao="Opcao (1-6): "

if "%opcao%"=="1" (
    echo.
    echo [INFO] Iniciando API Backend...
    echo API Backend rodara em: %BACKEND_URL%
    echo.
    cd /d "%BASE_DIR%\backend"
    call dotnet run
) else if "%opcao%"=="2" (
    echo.
    echo [INFO] Iniciando Web Frontend...
    echo Web Frontend rodara em: %FRONTEND_URL%
    echo.
    cd /d "%BASE_DIR%\frontend\app"
    call dotnet run
) else if "%opcao%"=="3" (
    echo.
    echo [INFO] Iniciando ambos os projetos...
    echo Abrindo em novas janelas...
    echo.
    
    REM Backend em nova janela
    echo Iniciando Backend em: %BACKEND_URL%
    start "ChatBot - Backend" cmd /k "cd /d %BASE_DIR%\backend && dotnet run"
    
    REM Aguardar um pouco para não sobrecarregar
    timeout /t 3 /nobreak
    
    REM Frontend em nova janela
    echo Iniciando Frontend em: %FRONTEND_URL%
    start "ChatBot - Frontend" cmd /k "cd /d %BASE_DIR%\frontend\app && dotnet run"
    
    echo.
    echo [OK] Ambos os projetos foram iniciados em novas janelas!
    echo.
    echo URLs:
    echo   API Backend:   %BACKEND_URL%
    echo   Web Frontend:  %FRONTEND_URL%
    echo   Swagger API:   %BACKEND_URL%/swagger
    echo.
    echo [INFO] Aguarde alguns segundos para inicializacao...
) else if "%opcao%"=="4" (
    echo.
    echo [INFO] Compilando API Backend...
    cd /d "%BASE_DIR%\backend"
    call dotnet build
    
    echo.
    echo [INFO] Compilando Web Frontend...
    cd /d "%BASE_DIR%\frontend\app"
    call dotnet build
    
    echo.
    echo [OK] Compilacao concluida!
) else if "%opcao%"=="5" (
    echo.
    echo [INFO] Limpando e reinstalando...
    
    echo [INFO] Limpando API Backend...
    cd /d "%BASE_DIR%\backend"
    call dotnet clean
    if exist "bin" rmdir /s /q "bin"
    if exist "obj" rmdir /s /q "obj"
    
    echo [INFO] Limpando Web Frontend...
    cd /d "%BASE_DIR%\frontend\app"
    call dotnet clean
    if exist "bin" rmdir /s /q "bin"
    if exist "obj" rmdir /s /q "obj"
    
    echo.
    echo [INFO] Restaurando pacotes...
    
    echo [INFO] Restaurando API Backend...
    cd /d "%BASE_DIR%\backend"
    call dotnet restore
    
    echo [INFO] Restaurando Web Frontend...
    cd /d "%BASE_DIR%\frontend\app"
    call dotnet restore
    
    echo.
    echo [OK] Limpeza e reinstalacao concluidas!
) else if "%opcao%"=="6" (
    echo.
    echo [INFO] Parando todos os processos dotnet...
    taskkill /F /IM dotnet.exe
    echo [OK] Processos parados
) else (
    echo [ERRO] Opcao invalida
    exit /b 1
)

echo.
echo [OK] Pronto!
pause
