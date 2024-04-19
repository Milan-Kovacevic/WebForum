FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /release
COPY ["src/WebForum.Api/WebForum.Api.csproj", "WebForum.Api/"]
RUN dotnet restore "./src/WebForum.Api/./WebForum.Api.csproj"
COPY . .
WORKDIR "/release/WebForum.Api"
RUN dotnet build "./WebForum.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebForum.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebForum.Api.dll"]
