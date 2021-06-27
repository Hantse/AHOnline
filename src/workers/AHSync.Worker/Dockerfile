#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/workers/AHSync.Worker/AHSync.Worker.csproj", "src/workers/AHSync.Worker/"]
COPY ["src/shared/AHSync.Worker.Shared/AHSync.Worker.Shared.csproj", "src/shared/AHSync.Worker.Shared/"]
COPY ["src/shared/Infrastructure.Core/Infrastructure.Core.csproj", "src/shared/Infrastructure.Core/"]
RUN dotnet restore "src/workers/AHSync.Worker/AHSync.Worker.csproj"
COPY . .
WORKDIR "/src/src/workers/AHSync.Worker"
RUN dotnet build "AHSync.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AHSync.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AHSync.Worker.dll"]