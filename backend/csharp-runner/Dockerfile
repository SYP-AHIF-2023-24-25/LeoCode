# Use the official Microsoft .NET SDK as a base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /usr/src/app

# Copy the project files into the container
COPY . .

# Build the C# project
RUN dotnet build -c Release -o /app

# Stage for the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the built files from the build stage
COPY --from=build /app .

# Expose the port the app will run on
EXPOSE 8000

# Define the entry point for the application
ENTRYPOINT ["dotnet", "csharp-runner.dll"]
