# Use an official OpenJDK image with Alpine as the base
FROM openjdk:alpine

# Add the JUnit library to the classpath
RUN apk add --no-cache junit

# Create directories
RUN mkdir -p /usr/src/work
RUN mkdir -p /usr/src/project

# Set working directory
WORKDIR /usr/src/work

# Copy the run.sh script
COPY run.sh run.sh

# Set execute permissions for the script
RUN chmod +x run.sh

# Set the script as the entry point
ENTRYPOINT ["/bin/sh", "run.sh"]

