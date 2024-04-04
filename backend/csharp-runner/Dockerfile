# Build-Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /usr/src/app

# Kopiere alles
COPY . ./

# Restore als separate Schichten
RUN dotnet restore
# Baue und veröffentliche ein Release
RUN dotnet publish -c Release -o out

# Runtime-Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS runtime
WORKDIR /usr/src/app
COPY --from=build-env /usr/src/app/out .
ENTRYPOINT ["dotnet", "csharp-runner.dll"]