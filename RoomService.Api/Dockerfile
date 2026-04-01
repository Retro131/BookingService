FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["RoomService.Api/RoomService.Api.csproj", "RoomService.Api/"]
COPY ["RoomService.Domain/RoomService.Domain.csproj", "RoomService.Domain/"]
COPY ["RoomService.Infrastructure/RoomService.Infrastructure.csproj", "RoomService.Infrastructure/"]
COPY ["RoomService.Handlers/RoomService.Handlers.csproj", "RoomService.Handlers/"]

RUN dotnet restore "RoomService.Api/RoomService.Api.csproj"

COPY . .

WORKDIR "/src/RoomService.Api"
RUN dotnet build "RoomService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RoomService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RoomService.Api.dll"]