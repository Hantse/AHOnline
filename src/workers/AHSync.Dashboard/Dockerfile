#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/workers/AHSync.Dashboard/AHSync.Dashboard.csproj", "src/workers/AHSync.Dashboard/"]
COPY ["src/shared/AHSync.Worker.Shared/AHSync.Worker.Shared.csproj", "src/shared/AHSync.Worker.Shared/"]
COPY ["src/shared/Infrastructure.Core/Infrastructure.Core.csproj", "src/shared/Infrastructure.Core/"]
RUN dotnet restore "src/workers/AHSync.Dashboard/AHSync.Dashboard.csproj"
COPY . .
WORKDIR "/src/src/workers/AHSync.Dashboard"
RUN dotnet build "AHSync.Dashboard.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AHSync.Dashboard.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AHSync.Dashboard.dll"]