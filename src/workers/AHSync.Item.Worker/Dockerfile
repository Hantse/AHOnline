#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/workers/AHSync.Item.Worker/AHSync.Item.Worker.csproj", "src/workers/AHSync.Item.Worker/"]
COPY ["src/shared/AHSync.Item.Worker.Shared/AHSync.Item.Worker.Shared.csproj", "src/shared/AHSync.Item.Worker.Shared/"]
RUN dotnet restore "src/workers/AHSync.Item.Worker/AHSync.Item.Worker.csproj"
COPY . .
WORKDIR "/src/src/workers/AHSync.Item.Worker"
RUN dotnet build "AHSync.Item.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AHSync.Item.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AHSync.Item.Worker.dll"]