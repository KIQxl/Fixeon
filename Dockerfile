# Estágio 1: Build (Otimizado para Cache)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copia APENAS os arquivos de projeto (.csproj)
# Isso permite que o Docker use o cache para o 'dotnet restore'
# a menos que as dependências (pacotes NuGet) mudem.
COPY ["./src/web/Fixeon.WebApi/Fixeon.WebApi.csproj", "src/web/Fixeon.WebApi/"]
RUN dotnet restore "src/web/Fixeon.WebApi/Fixeon.WebApi.csproj"

# 2. Copia o restante do código-fonte
COPY . .

# 3. Publica a aplicação
WORKDIR "/src/web/Fixeon.WebApi"
RUN dotnet publish "Fixeon.WebApi.csproj" -c Release -o /app/publish --no-restore

# Estágio 2: Runtime (Otimizado para Nuvem)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# 1. Expõe a porta 8080 (Melhor Prática para Render)
EXPOSE 8080

# 2. Define a variável de ambiente para a porta (Garante que o Kestrel escute na porta 8080)
ENV ASPNETCORE_URLS=http://+:8080

# 3. Comando de inicialização
ENTRYPOINT ["dotnet", "Fixeon.WebApi.dll"]
