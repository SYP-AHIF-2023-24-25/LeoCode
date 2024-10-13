#!/bin/bash

language="$1";
project_name="$2";

mkdir -p /usr/cache
mkdir -p /usr/src/work/results
mkdir -p /usr/src/project/"$language"/"$project_name"/results
cp -R /usr/src/project/"$language"/"$project_name"/* /usr/src/work
cd /usr/src/work || exit 1

if [ "$language" == "CSharp" ]; then
    dotnet restore
    dotnet test -l:trx;LogFileName=TestOutput.xml
    cp -r /usr/src/work/"$project_name"Tests/TestResults/* /usr/src/project/"$language"/"$project_name"/results
elif [ "$language" == "Typescript" ]; then
    npm install
    tsc
    npm test -- --reporter json --reporter-options output=/usr/src/work/results/testresults.json
    cp -r /usr/src/work/results/* /usr/src/project/"$language"/"$project_name"/results
elif [ "$language" == "Java" ]; then
    mvn test surefire-report:report
    cp -r target/surefire-reports/* /usr/src/project/"$language"/"$project_name"/results
fi

echo "Build und Testprozess abgeschlossen."