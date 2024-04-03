FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /usr/src/app

# Kopiere alles
COPY . ./

# Setze Berechtigungen für alle Dateien und Ordner im Arbeitsverzeichnis
RUN chmod -R 755 .

# Restore als separate Schichten
RUN dotnet restore
# Baue und veröffentliche ein Release
RUN dotnet publish -c Release -o out

# Build des Laufzeitimages
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /usr/src/app
COPY --from=build-env /usr/src/app/out .
RUN chmod -R 755 .
ENTRYPOINT ["dotnet", "csharp-runner.dll"]