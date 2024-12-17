@echo off
set "diretorio_atual=%CD%"

echo Inicializando o Docker Compose...
docker compose up -d --build

echo Aguardando 20 segundos para estabilizacao...
for /l %%i in (20,-1,1) do (
    <nul set /p "=Restam %%i segundos... "
    timeout /t 1 /nobreak > nul
    echo.
)

echo Reinicializando o Docker Compose...
docker compose restart taskmanager

echo Abrindo o Swagger no navegador... Aguarde...
timeout /t 10 /nobreak > nul
start "TaskManager" http://localhost:8089/swagger

echo Script concluído.
