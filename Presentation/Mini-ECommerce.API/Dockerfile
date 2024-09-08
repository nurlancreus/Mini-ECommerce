#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Presentation/Mini-ECommerce.API/Mini-ECommerce.API.csproj", "Presentation/Mini-ECommerce.API/"]
COPY ["Infrastructure/Mini-ECommerce.Infrastructure/Mini-ECommerce.Infrastructure.csproj", "Infrastructure/Mini-ECommerce.Infrastructure/"]
COPY ["Core/Mini-ECommerce.Application/Mini-ECommerce.Application.csproj", "Core/Mini-ECommerce.Application/"]
COPY ["Core/Mini-ECommerce.Domain/Mini-ECommerce.Domain.csproj", "Core/Mini-ECommerce.Domain/"]
COPY ["Infrastructure/Mini-ECommerce.Persistence/Mini-ECommerce.Persistence.csproj", "Infrastructure/Mini-ECommerce.Persistence/"]
COPY ["Infrastructure/Mini-ECommerce.SignalR/Mini-ECommerce.SignalR.csproj", "Infrastructure/Mini-ECommerce.SignalR/"]
RUN dotnet restore "./Presentation/Mini-ECommerce.API/Mini-ECommerce.API.csproj"
COPY . .
WORKDIR "/src/Presentation/Mini-ECommerce.API"
RUN dotnet build "./Mini-ECommerce.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Mini-ECommerce.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mini-ECommerce.API.dll"]