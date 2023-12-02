#!/bin/bash

language="$1";
project_name="$2";

mkdir -p /usr/cache
mkdir -p /usr/src/work/results
mkdir -p /usr/src/project/results

cp -R /usr/src/project/"$language"/"$project_name"/* /usr/src/work

cd /usr/src/work || exit 1

# NuGet-Pakete wiederherstellen
dotnet restore

# Tests ausführen und Ergebnisse in TestOutput.xml speichern
dotnet test -l:trx;LogFileName=TestOutput.xml

# Testergebnisse von MultiplikatorTests nach /usr/src/project/results kopieren
cp -r /usr/src/work/"$project_name"Tests/TestResults/* /usr/src/project/results

echo "Build und Testprozess abgeschlossen."