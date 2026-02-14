FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Lemax.sln", "./"]
COPY ["src/Lemax.API/Lemax.API.csproj", "src/Lemax.API/"]
COPY ["src/Lemax.Application/Lemax.Application.csproj", "src/Lemax.Application/"]
COPY ["src/Lemax.Domain/Lemax.Domain.csproj", "src/Lemax.Domain/"]
COPY ["src/Lemax.Infrastructure/Lemax.Infrastructure.csproj", "src/Lemax.Infrastructure/"]
COPY ["src/Lemax.Shared/Lemax.Shared.csproj", "src/Lemax.Shared/"]
COPY ["src/Lemax.SQL/Lemax.SQL.csproj", "src/Lemax.SQL/"]
COPY ["src/UnitTest/UnitTest.csproj", "src/UnitTest/"] 

RUN dotnet restore

COPY . .
WORKDIR "/src/src/Lemax.API"
RUN dotnet build "Lemax.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Lemax.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lemax.API.dll"]