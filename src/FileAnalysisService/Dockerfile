FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FileAnalysisService.Web/FileAnalysisService.Web.csproj", "FileAnalysisService.Web/"]
COPY ["FileAnalysisService.UseCases/FileAnalysisService.UseCases.csproj", "FileAnalysisService.UseCases/"]
COPY ["FileAnalysisService.Core/FileAnalysisService.Core.csproj", "FileAnalysisService.Core/"]
COPY ["FileAnalysisService.Infrastructure/FileAnalysisService.Infrastructure.csproj", "FileAnalysisService.Infrastructure/"]
RUN dotnet restore "FileAnalysisService.Web/FileAnalysisService.Web.csproj"
COPY . .
WORKDIR "/src/FileAnalysisService.Web"
RUN dotnet build "./FileAnalysisService.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FileAnalysisService.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileAnalysisService.Web.dll"]
