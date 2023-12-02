FROM node:latest

WORKDIR /usr/src/project

# Installiere notwendige Abhängigkeiten
RUN npm install -g typescript

ENV NUGET_PACKAGES=/usr/cache 

#RUN ["apk", "add", "cp"] # alpine stuff

RUN ["mkdir", "-p", "/usr/src/work"]
RUN ["mkdir", "-p", "/usr/src/project"]

WORKDIR /usr/src/work

COPY run.sh /scripts/run.sh
RUN ["chmod", "+x", "/scripts/run.sh"]

ENTRYPOINT ["/scripts/run.sh"]
