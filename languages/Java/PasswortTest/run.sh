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
output_dir="/usr/src/project/results"
project_path="$1"
folder_name="$2"
output_file="/usr/src/project/results/test_results.txt"

# Projektordner erstellen und in das Projektverzeichnis wechseln
mkdir -p /usr/cache
mkdir -p /usr/src/work/results
mkdir -p /usr/src/project/results
cp -R "$project_path"/* /usr/src/work
cd /usr/src/work || exit 1

cd ./src
echo $(pwd)
echo $(ls)
javac PasswortTest.java

# Check if compilation was successful
if [ $? -eq 0 ]; then
  echo "Compilation successful. Running the program..."

  # Run Java program
  java PasswortTest

else
  echo "Compilation failed. Please fix the errors."
fi
echo $(pwd)
echo $(ls)

# Compile the JUnit test file with dependencies in the classpath
javac -cp .:junit-jupiter-api-5.10.1.jar:junit-jupiter-engine-5.10.1.jar:junit-platform-console-standalone-1.9.3.jar:apiguardian-api-1.1.2.jar PasswortTestTests.java

# Run the compiled JUnit test (replace YourTestClassName with the actual test class name)
java -cp .:junit-jupiter-api-5.10.1.jar:junit-jupiter-engine-5.10.1.jar:junit-platform-console-standalone-1.9.3.jar:apiguardian-api-1.1.2.jar org.junit.platform.console.ConsoleLauncher --select-class PasswortTestTests > $output_file 2>&1 && echo "Tests passed successfully!"

cp -r /usr/src/work/results/* /usr/src/project/results
