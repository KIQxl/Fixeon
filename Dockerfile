# Stage 1: Build (Otimizado para Cache)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copia APENAS os arquivos de projeto (.csproj)
# O caminho agora inclui a pasta Fixeon/
COPY ["./Fixeon/src/web/Fixeon.WebApi/Fixeon.WebApi.csproj", "Fixeon/src/web/Fixeon.WebApi/"]
RUN dotnet restore "Fixeon/src/web/Fixeon.WebApi/Fixeon.WebApi.csproj"

# 2. Copia o restante do código-fonte
COPY . .

# 3. Publica a aplicação
# O WORKDIR deve ser o caminho do projeto DENTRO do contêiner (/src + caminho real)
WORKDIR "/src/Fixeon/src/web/Fixeon.WebApi" 
RUN dotnet publish "Fixeon.WebApi.csproj" -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Fixeon.WebApi.dll"]
