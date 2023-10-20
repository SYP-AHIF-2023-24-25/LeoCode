#!/bin/bash

#echo "in bash"

mkdir -p /usr/cache
mkdir -p /usr/src/work/results
mkdir -p /usr/src/project/results

cp -R /usr/src/project/* /usr/src/work

#dotnet restore --packages /usr/cache
dotnet test -l:trx;LogFileName=TestOutput.xml --no-restore

cp /usr/src/work/TestResults/* /usr/src/project/results