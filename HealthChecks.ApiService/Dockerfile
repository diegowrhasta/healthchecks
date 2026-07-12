FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HealthChecks.ApiService/HealthChecks.ApiService.csproj", "HealthChecks.ApiService/"]
COPY ["HealthChecks.ServiceDefaults/HealthChecks.ServiceDefaults.csproj", "HealthChecks.ServiceDefaults/"]
RUN dotnet restore "HealthChecks.ApiService/HealthChecks.ApiService.csproj"
COPY . .
WORKDIR "/src/HealthChecks.ApiService"
RUN dotnet build "./HealthChecks.ApiService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HealthChecks.ApiService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthChecks.ApiService.dll"]
