FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ./Base/Base.csproj ./Base/
COPY ./Core/Core.csproj ./Core/
COPY ./Persistence/Persistence.csproj ./Persistence/
COPY ./WebAPI/WebAPI.csproj ./WebAPI/
RUN dotnet restore ./WebAPI/WebAPI.csproj

# Copy everything else and build
COPY . .
RUN dotnet publish ./WebAPI/WebAPI.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Production
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]