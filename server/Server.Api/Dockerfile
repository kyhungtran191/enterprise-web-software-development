#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Server.Api/Server.Api.csproj", "Server.Api/"]
COPY ["Server.Contracts/Server.Contracts.csproj", "Server.Contracts/"]
COPY ["Server.Infrastructure/Server.Infrastructure.csproj", "Server.Infrastructure/"]
COPY ["Server.Application/Server.Application.csproj", "Server.Application/"]
COPY ["Server.Domain/Server.Domain.csproj", "Server.Domain/"]
RUN dotnet restore "./Server.Api/Server.Api.csproj"
COPY . .
WORKDIR "/src/Server.Api"
RUN dotnet build "./Server.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Server.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Server.Api.dll"]