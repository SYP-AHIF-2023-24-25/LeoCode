# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ./csharp-runner.csproj .
RUN dotnet restore

# Copy the rest of the application and publish
COPY . .
RUN dotnet publish ./csharp-runner.csproj -c Production -o /app/out

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the output from the build stage


# Expose ports for production and debugging
EXPOSE 5168
ENV ASPNETCORE_ENVIRONMENT=Production
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "csharp-runner.dll"]