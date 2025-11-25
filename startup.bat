@echo off
REM Script para iniciar todos os projetos - API Backend, Web Frontend e Desktop (Windows)

setlocal enabledelayedexpansion

echo.
echo ========================================================
echo          ChatBot - Startup Script (Windows)
echo   Inicia API Backend, Web Frontend e Desktop
echo ========================================================
echo.

REM Detectar diretório onde o script está localizado
set "BASE_DIR=%~dp0"
REM Remover barra final se existir
if "%BASE_DIR:~-1%"=="\" set "BASE_DIR=%BASE_DIR:~0,-1%"

REM Valores padrão
set "BACKEND_URL=http://localhost:6660"
set "FRONTEND_URL=http://localhost:6661"
set "BACKEND_PORT=6660"
set "FRONTEND_PORT=6661"
set "FRONTEND_API_BASE_URL=http://localhost:6660"
set "GOOGLE_GEMINI_API_KEY="
set "CONNECTION_STRING="

REM Ler arquivo .env se existir
if exist "%BASE_DIR%\.env" (
    echo [INFO] Carregando arquivo .env...
    for /f "usebackq eol=# delims==" %%a in ("%BASE_DIR%\.env") do (
        if not "%%a"=="" (
            set "%%a=%%b"
        )
    )
)

REM Se variáveis chave não foram lidas, usar localhost
if "!BACKEND_URL!"=="" set "BACKEND_URL=http://localhost:!BACKEND_PORT!"
if "!FRONTEND_URL!"=="" set "FRONTEND_URL=http://localhost:!FRONTEND_PORT!"
if "!FRONTEND_API_BASE_URL!"=="" set "FRONTEND_API_BASE_URL=!BACKEND_URL!"

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

if not exist "%BASE_DIR%\desktop" (
    echo [ERRO] Diretório desktop não encontrado em: %BASE_DIR%\desktop
    pause
    exit /b 1
)

echo [OK] API Backend encontrada
echo [OK] Web Frontend encontrada
echo [OK] Desktop encontrado
echo.
echo URLs Configuradas:
echo   Backend:  %BACKEND_URL%
echo   Frontend: %FRONTEND_URL%
echo.
echo Variaveis de Ambiente:
if "%FRONTEND_API_BASE_URL%"=="" (
    echo   FRONTEND_API_BASE_URL: (nao definida - usando backend)
) else (
    echo   FRONTEND_API_BASE_URL: %FRONTEND_API_BASE_URL%
)
if "%GOOGLE_GEMINI_API_KEY%"=="" (
    echo   GOOGLE_GEMINI_API_KEY: (nao definida)
) else (
    echo   GOOGLE_GEMINI_API_KEY: (definida)
)
echo.

:mainMenu
echo ===============================================================
echo                     MENU PRINCIPAL
echo ===============================================================
echo.
echo 1. [PADRAO] Iniciar todos (Backend, Frontend e Desktop)
echo 2. Opcoes avancadas
echo 3. Parar todos os processos
echo.

set /p opcao="Escolha uma opcao (1-3): "

if "%opcao%"=="1" (
    goto rodarTodos
) else if "%opcao%"=="2" (
    goto opcoesAvancadas
) else if "%opcao%"=="3" (
    echo.
    echo [INFO] Parando todos os processos dotnet...
    taskkill /F /IM dotnet.exe >nul 2>&1
    echo [OK] Processos parados
    pause
    exit /b 0
) else (
    echo [ERRO] Opcao invalida
    echo.
    goto mainMenu
)

:rodarTodos
echo.
echo [INFO] Iniciando todos os projetos...
echo Abrindo em novas janelas...
echo.

REM Copiar .env para todos os diretórios
if exist "%BASE_DIR%\.env" (
    copy "%BASE_DIR%\.env" "%BASE_DIR%\backend\.env" >nul 2>&1
    copy "%BASE_DIR%\.env" "%BASE_DIR%\frontend\app\.env" >nul 2>&1
    copy "%BASE_DIR%\.env" "%BASE_DIR%\desktop\.env" >nul 2>&1
)

REM Backend em nova janela
echo Iniciando Backend em: %BACKEND_URL%
start "ChatBot - Backend" cmd /k "cd /d %BASE_DIR%\backend && set ASPNETCORE_URLS=http://0.0.0.0:%BACKEND_PORT% && dotnet run"

REM Aguardar um pouco para não sobrecarregar
timeout /t 3 /nobreak

REM Frontend em nova janela
echo Iniciando Frontend em: %FRONTEND_URL%
start "ChatBot - Frontend" cmd /k "cd /d %BASE_DIR%\frontend\app && set ASPNETCORE_URLS=http://0.0.0.0:%FRONTEND_PORT% && set FRONTEND_API_BASE_URL=%FRONTEND_API_BASE_URL% && dotnet run"

REM Aguardar um pouco
timeout /t 3 /nobreak

REM Desktop em nova janela
echo Iniciando Desktop...
start "ChatBot - Desktop" cmd /k "cd /d %BASE_DIR%\desktop && dotnet run --configuration Release"

echo.
echo ===============================================================
echo [OK] Todos os projetos foram iniciados em novas janelas!
echo ===============================================================
echo.
echo URLs:
echo   API Backend:   %BACKEND_URL%
echo   Web Frontend:  %FRONTEND_URL%
echo   Swagger API:   %BACKEND_URL%/swagger
echo.
echo [INFO] Aguarde alguns segundos para inicializacao...
echo.
pause
exit /b 0

:opcoesAvancadas
cls
echo.
echo ========================================================
echo              OPCOES AVANCADAS
echo ========================================================
echo.
echo 1. Iniciar apenas API Backend
echo 2. Iniciar apenas Web Frontend
echo 3. Iniciar apenas Desktop
echo 4. Iniciar Backend e Frontend (sem Desktop)
echo 5. Iniciar Backend e Desktop (sem Frontend)
echo 6. Iniciar Frontend e Desktop (sem Backend)
echo 7. Compilar todos os projetos
echo 8. Limpar e reinstalar dependencias
echo 9. Voltar ao menu principal
echo.

set /p opcao_avancada="Escolha uma opcao (1-9): "

if "%opcao_avancada%"=="1" (
    echo.
    echo [INFO] Iniciando API Backend...
    echo API Backend rodara em: %BACKEND_URL%
    echo.
    cd /d "%BASE_DIR%\backend"
    if exist "%BASE_DIR%\.env" (
        copy "%BASE_DIR%\.env" "%BASE_DIR%\backend\.env" >nul 2>&1
    )
    set "ASPNETCORE_URLS=http://0.0.0.0:%BACKEND_PORT%"
    call dotnet run
) else if "%opcao_avancada%"=="2" (
    echo.
    echo [INFO] Iniciando Web Frontend...
    echo Web Frontend rodara em: %FRONTEND_URL%
    echo.
    cd /d "%BASE_DIR%\frontend\app"
    if exist "%BASE_DIR%\.env" (
        copy "%BASE_DIR%\.env" "%BASE_DIR%\frontend\app\.env" >nul 2>&1
    )
    set "ASPNETCORE_URLS=http://0.0.0.0:%FRONTEND_PORT%"
    set "FRONTEND_API_BASE_URL=%FRONTEND_API_BASE_URL%"
    call dotnet run
) else if "%opcao_avancada%"=="3" (
    echo.
    echo [INFO] Iniciando Desktop...
    echo.
    cd /d "%BASE_DIR%\desktop"
    call dotnet run --configuration Release
) else if "%opcao_avancada%"=="4" (
    echo.
    echo [INFO] Iniciando Backend e Frontend (sem Desktop)...
    echo Abrindo em novas janelas...
    echo.
    
    if exist "%BASE_DIR%\.env" (
        copy "%BASE_DIR%\.env" "%BASE_DIR%\backend\.env" >nul 2>&1
        copy "%BASE_DIR%\.env" "%BASE_DIR%\frontend\app\.env" >nul 2>&1
    )
    
    echo Iniciando Backend em: %BACKEND_URL%
    start "ChatBot - Backend" cmd /k "cd /d %BASE_DIR%\backend && set ASPNETCORE_URLS=http://0.0.0.0:%BACKEND_PORT% && dotnet run"
    
    timeout /t 3 /nobreak
    
    echo Iniciando Frontend em: %FRONTEND_URL%
    start "ChatBot - Frontend" cmd /k "cd /d %BASE_DIR%\frontend\app && set ASPNETCORE_URLS=http://0.0.0.0:%FRONTEND_PORT% && set FRONTEND_API_BASE_URL=%FRONTEND_API_BASE_URL% && dotnet run"
    
    echo.
    echo [OK] Backend e Frontend foram iniciados em novas janelas!
    echo URLs: Backend %BACKEND_URL% - Frontend %FRONTEND_URL%
    echo.
    pause
) else if "%opcao_avancada%"=="5" (
    echo.
    echo [INFO] Iniciando Backend e Desktop (sem Frontend)...
    echo Abrindo em novas janelas...
    echo.
    
    if exist "%BASE_DIR%\.env" (
        copy "%BASE_DIR%\.env" "%BASE_DIR%\backend\.env" >nul 2>&1
        copy "%BASE_DIR%\.env" "%BASE_DIR%\desktop\.env" >nul 2>&1
    )
    
    echo Iniciando Backend em: %BACKEND_URL%
    start "ChatBot - Backend" cmd /k "cd /d %BASE_DIR%\backend && set ASPNETCORE_URLS=http://0.0.0.0:%BACKEND_PORT% && dotnet run"
    
    timeout /t 3 /nobreak
    
    echo Iniciando Desktop...
    start "ChatBot - Desktop" cmd /k "cd /d %BASE_DIR%\desktop && dotnet run --configuration Release"
    
    echo.
    echo [OK] Backend e Desktop foram iniciados em novas janelas!
    echo.
    pause
) else if "%opcao_avancada%"=="6" (
    echo.
    echo [INFO] Iniciando Frontend e Desktop (sem Backend)...
    echo Abrindo em novas janelas...
    echo.
    
    if exist "%BASE_DIR%\.env" (
        copy "%BASE_DIR%\.env" "%BASE_DIR%\frontend\app\.env" >nul 2>&1
        copy "%BASE_DIR%\.env" "%BASE_DIR%\desktop\.env" >nul 2>&1
    )
    
    echo Iniciando Frontend em: %FRONTEND_URL%
    start "ChatBot - Frontend" cmd /k "cd /d %BASE_DIR%\frontend\app && set ASPNETCORE_URLS=http://0.0.0.0:%FRONTEND_PORT% && set FRONTEND_API_BASE_URL=%FRONTEND_API_BASE_URL% && dotnet run"
    
    timeout /t 3 /nobreak
    
    echo Iniciando Desktop...
    start "ChatBot - Desktop" cmd /k "cd /d %BASE_DIR%\desktop && dotnet run --configuration Release"
    
    echo.
    echo [OK] Frontend e Desktop foram iniciados em novas janelas!
    echo.
    pause
) else if "%opcao_avancada%"=="7" (
    echo.
    echo [INFO] Compilando todos os projetos...
    if exist "%BASE_DIR%\.env" (
        copy "%BASE_DIR%\.env" "%BASE_DIR%\backend\.env" >nul 2>&1
        copy "%BASE_DIR%\.env" "%BASE_DIR%\frontend\app\.env" >nul 2>&1
    )
    
    echo [INFO] Compilando API Backend...
    cd /d "%BASE_DIR%\backend"
    call dotnet build
    
    echo.
    echo [INFO] Compilando Web Frontend...
    cd /d "%BASE_DIR%\frontend\app"
    call dotnet build
    
    echo.
    echo [INFO] Compilando Desktop...
    cd /d "%BASE_DIR%\desktop"
    call dotnet build -c Release
    
    echo.
    echo [OK] Compilacao concluida!
    pause
) else if "%opcao_avancada%"=="8" (
    echo.
    echo [INFO] Limpando e reinstalando todos os projetos...
    
    echo [INFO] Limpando API Backend...
    cd /d "%BASE_DIR%\backend"
    call dotnet clean
    if exist "bin" rmdir /s /q "bin" >nul 2>&1
    if exist "obj" rmdir /s /q "obj" >nul 2>&1
    
    echo [INFO] Limpando Web Frontend...
    cd /d "%BASE_DIR%\frontend\app"
    call dotnet clean
    if exist "bin" rmdir /s /q "bin" >nul 2>&1
    if exist "obj" rmdir /s /q "obj" >nul 2>&1
    
    echo [INFO] Limpando Desktop...
    cd /d "%BASE_DIR%\desktop"
    call dotnet clean
    if exist "bin" rmdir /s /q "bin" >nul 2>&1
    if exist "obj" rmdir /s /q "obj" >nul 2>&1
    
    echo.
    echo [INFO] Restaurando pacotes...
    
    echo [INFO] Restaurando API Backend...
    if exist "%BASE_DIR%\.env" (
        copy "%BASE_DIR%\.env" "%BASE_DIR%\backend\.env" >nul 2>&1
    )
    cd /d "%BASE_DIR%\backend"
    call dotnet restore
    
    echo [INFO] Restaurando Web Frontend...
    if exist "%BASE_DIR%\.env" (
        copy "%BASE_DIR%\.env" "%BASE_DIR%\frontend\app\.env" >nul 2>&1
    )
    cd /d "%BASE_DIR%\frontend\app"
    call dotnet restore
    
    echo [INFO] Restaurando Desktop...
    cd /d "%BASE_DIR%\desktop"
    call dotnet restore
    
    echo.
    echo [OK] Limpeza e reinstalacao concluidas!
    pause
) else if "%opcao_avancada%"=="9" (
    cls
    goto mainMenu
) else (
    echo [ERRO] Opcao invalida
    echo.
    pause
    cls
    goto opcoesAvancadas
)

echo.
pause
exit /b 0
