# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
# CORREÇÃO: Apontar o dotnet restore para o caminho correto do arquivo .sln
RUN dotnet restore Fixeon/Fixeon.sln 
# CORREÇÃO: Apontar o dotnet publish para o caminho correto do .csproj
RUN dotnet publish ./Fixeon/src/web/Fixeon.WebApi/Fixeon.WebApi.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Fixeon.WebApi.dll"]
