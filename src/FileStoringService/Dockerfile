FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FileStoringService.Web/FileStoringService.Web.csproj", "FileStoringService.Web/"]
COPY ["FileStoringService.Infrastructure/FileStoringService.Infrastructure.csproj", "FileStoringService.Infrastructure/"]
COPY ["FileStoringService.UseCases/FileStoringService.UseCases.csproj", "FileStoringService.UseCases/"]
COPY ["FileStoringService.Core/FileStoringService.Core.csproj", "FileStoringService.Core/"]
RUN dotnet restore "FileStoringService.Web/FileStoringService.Web.csproj"
COPY . .
WORKDIR "/src/FileStoringService.Web"
RUN dotnet build "./FileStoringService.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FileStoringService.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileStoringService.Web.dll"]
