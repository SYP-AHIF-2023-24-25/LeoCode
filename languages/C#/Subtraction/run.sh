#!/bin/bash

# Überprüfen, ob der Pfad zum Projektordner als Argument übergeben wurde
if [ -z "$1" ]; then
    echo "Bitte den Pfad zum Projektordner als 1 Argument angeben."
    exit 1
fi

if [ -z "$2" ]; then
    echo "Bitte den Ordnernamen als 2 Argument angeben."
    exit 1
fi

project_path="$1"
folder_name="$2"

# Projektordner erstellen und in das Projektverzeichnis wechseln
mkdir -p /usr/cache
mkdir -p /usr/src/work/results
mkdir -p /usr/src/project/results
cp -R "$project_path"/* /usr/src/work
cd /usr/src/work || exit 1

# NuGet-Pakete wiederherstellen
dotnet restore

# Projekt erstellen (falls benötigt)
#dotnet build

# Tests ausführen und Ergebnisse in TestOutput.xml speichern
dotnet test -l:trx;LogFileName=TestOutput.xml

# Testergebnisse von MultiplikatorTests nach /usr/src/project/results kopieren
cp -r /usr/src/work/"$folder_name"Tests/TestResults/* /usr/src/project/results

echo "Build und Testprozess abgeschlossen."