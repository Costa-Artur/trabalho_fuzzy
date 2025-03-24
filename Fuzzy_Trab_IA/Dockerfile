FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Fuzzy_Trab_IA/Fuzzy_Trab_IA.csproj", "Fuzzy_Trab_IA/"]
RUN dotnet restore "Fuzzy_Trab_IA/Fuzzy_Trab_IA.csproj"
COPY . .
WORKDIR "/src/Fuzzy_Trab_IA"
RUN dotnet build "Fuzzy_Trab_IA.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Fuzzy_Trab_IA.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fuzzy_Trab_IA.dll"]
