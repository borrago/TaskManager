# Define a imagem base
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Define o diretório de trabalho dentro do contêiner
WORKDIR /app

# Copia os arquivos de projeto para o diretório de trabalho
COPY src/TaskManager.Core/TaskManager.Core.csproj src/TaskManager.Core/
COPY src/TaskManager.Domain/TaskManager.Domain.csproj src/TaskManager.Domain/
COPY src/TaskManager.Infra/TaskManager.Infra.csproj src/TaskManager.Infra/
COPY src/TaskManager.Application/TaskManager.Application.csproj src/TaskManager.Application/
COPY src/TaskManager.API/TaskManager.API.csproj src/TaskManager.API/

# Restaura as dependências dos projetos
RUN dotnet restore src/TaskManager.API/TaskManager.API.csproj

# Copia todo o código-fonte para o diretório de trabalho
COPY . ./

# Restaurar pacotes NuGet
RUN dotnet restore src/TaskManager.API/TaskManager.API.csproj

# Publicar o projeto
RUN dotnet publish src/TaskManager.API/TaskManager.API.csproj -c Release -o /app/out

# Define a imagem base para a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Define o diretório de trabalho dentro do contêiner
WORKDIR /app

# Copia os arquivos publicados para o diretório de trabalho
COPY --from=build /app/out ./

# Define a variável de ambiente para configurar a data e hora para América/São Paulo
ENV TZ=America/Sao_Paulo

# Define a variável de ambiente para configurar o idioma para pt-BR
ENV LANG=pt_BR.UTF-8

# Define o comando de inicialização da aplicação
ENTRYPOINT ["dotnet", "TaskManager.API.dll"]
