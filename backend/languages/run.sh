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
    output_dir="/usr/src/project/"$language"/"$project_name"/results"
    output_file="/usr/src/project/"$language"/"$project_name"/results/test_results.txt"
    cd ./src
    javac "$project_name".java
    java "$project_name"
    javac -cp .:junit-jupiter-api-5.10.1.jar:junit-jupiter-engine-5.10.1.jar:junit-platform-console-standalone-1.9.3.jar:apiguardian-api-1.1.2.jar "$project_name"Tests.java
    java -cp .:junit-jupiter-api-5.10.1.jar:junit-jupiter-engine-5.10.1.jar:junit-platform-console-standalone-1.9.3.jar:apiguardian-api-1.1.2.jar org.junit.platform.console.ConsoleLauncher --select-class "$project_name"Tests > $output_file 2>&1 && echo "Tests passed successfully!"
    cp -r /usr/src/work/results/* /usr/src/project/results
fi

echo "Build und Testprozess abgeschlossen."