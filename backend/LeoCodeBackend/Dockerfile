# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./LeoCodeBackend.csproj .
RUN dotnet restore

# Copy the rest of the application and publish
COPY . .
RUN dotnet publish ./LeoCodeBackend.csproj -c Release -o /app/out

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the output from the build stage
COPY --from=build /app/out .

# Expose ports for production and debugging
EXPOSE 8081
ENV ASPNETCORE_URLS=http://+:8081
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "LeoCodeBackend.dll"]