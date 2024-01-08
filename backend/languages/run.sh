#!/bin/bash

language="$1";
project_name="$2";

mkdir -p /usr/cache
#mkdir -p /usr/src/work/results
mkdir -p /usr/src/project/"$language"/"$project_name"/results

#cp -R /usr/src/project/"$language"/"$project_name"/* /usr/src/work
#cd /usr/src/work || exit 1
cd /usr/src/project/"$language"/"$project_name"

if [ "$language" == "CSharp" ]; then
    dotnet restore
    dotnet test -l:trx;LogFileName=TestOutput.xml
    cp -r /usr/src/work/"$project_name"Tests/TestResults/* /usr/src/project/"$language"/"$project_name"/results
elif [ "$language" == "Typescript" ]; then
    echo "in typescript drinnen"
    tsc 
    echo "code kompiliert"
    echo "akutelles Verzeichnis: $(pwd)"
    npm test -- --reporter json --reporter-options output=/usr/src/project/"$language"/"$project_name"/results/testresults.json
    echo "tests durchgeführt"
    #cp -r /usr/src/work/results/* /usr/src/project/"$language"/"$project_name"/results
    echo "testresults kopiert"
elif [ "$language" == "Java" ]; then
    mvn test surefire-report:report
    cp -r target/surefire-reports/* /usr/src/project/"$language"/"$project_name"/results
fi

echo "Build und Testprozess abgeschlossen."