# Verwende ein offizielles Maven-Image als Basisimage
FROM maven:3.9.5-amazoncorretto-17 as builder

ENV NUGET_PACKAGES=/usr/cache 

#RUN ["apk", "add", "cp"] # alpine stuff

RUN ["mkdir", "-p", "/usr/src/work"]
RUN ["mkdir", "-p", "/usr/src/project"]

WORKDIR /usr/src/work

COPY run.sh /scripts/run.sh
RUN ["chmod", "+x", "/scripts/run.sh"]

ENTRYPOINT ["/scripts/run.sh"]