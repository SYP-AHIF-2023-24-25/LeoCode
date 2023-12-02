# Use an official OpenJDK image with Alpine as the base
FROM openjdk:alpine

# Add the JUnit library to the classpath
RUN apk add --no-cache junit

#RUN ["apk", "add", "cp"] # alpine stuff

RUN ["mkdir", "-p", "/usr/src/work"]
RUN ["mkdir", "-p", "/usr/src/project"]

WORKDIR /usr/src/work

COPY run.sh /scripts/run.sh
RUN ["chmod", "+x", "/scripts/run.sh"]

ENTRYPOINT ["/scripts/run.sh"]




