# Verwende das offizielle .NET SDK-Basisbild, um das Projekt zu erstellen
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Setze das Arbeitsverzeichnis
WORKDIR /src

# Kopiere die spezifische .csproj-Datei und stelle die Abhängigkeiten wieder her
COPY AuthDemoApi.csproj ./
RUN dotnet restore AuthDemoApi.csproj

# Kopiere den gesamten Code und baue das Projekt
COPY . .
RUN dotnet build AuthDemoApi.csproj -c Release -o /app/build

# Veröffentliche das Projekt
RUN dotnet publish AuthDemoApi.csproj -c Release -o /app/publish

# Verwende ein schlankeres .NET-Image zum Ausführen der Web-API
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Kopiere die veröffentlichten Dateien aus dem Build-Image
COPY --from=build /app/publish .

EXPOSE 5001

# Setze den Einstiegspunkt
ENTRYPOINT ["dotnet", "AuthDemoApi.dll"]
