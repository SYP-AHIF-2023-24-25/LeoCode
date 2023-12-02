#!/bin/bash

echo "Building and testing TypeScript project"

language="$1";
project_name="$2";

# Erstelle Verzeichnisse
echo "Creating directories"
mkdir -p /usr/cache
mkdir -p /usr/src/work/results
mkdir -p /usr/src/project/"$language"/"$project_name"/results

echo "Copying files"
# Kopiere Projektdateien
cp -R /usr/src/project/"$language"/"$project_name"/* /usr/src/work
cd /usr/src/work || exit 1

echo "Installing dependencies"
# Installiere npm-Abhängigkeiten
npm install

echo "Compile TypeScript"
# Kompiliere TypeScript-Code
tsc

echo " Running and saving test results"

#Führe Tests aus und speichere die Testergebnisse
npm test -- --reporter json --reporter-options output=/usr/src/work/results/testresults.json

echo "Copying test results"
# Kopiere Testergebnisse
if [ -d "/usr/src/work/results/" ]; then
    cp -r /usr/src/work/results/* /usr/src/project/"$language"/"$project_name"/results
else
    echo "Quellverzeichnis '/usr/src/work/results/' nicht gefunden oder leer."
fi
