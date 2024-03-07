# Use an official OpenJDK 17 runtime as the base image

FROM openjdk:17


# Set the working directory to /app

WORKDIR /app


# Copy the current directory contents into the container at /app

COPY . /app


# Set the default JAVA_OPTS environment variable

ENV JAVA_OPTS="-Xms512M -Xmx512M"


# Run the command to start the Quarkus application

CMD ["java", "-jar", "target/quarkus-app/quarkus-run.jar"]