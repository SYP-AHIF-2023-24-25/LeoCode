#!/bin/bash

language="$1";
project_name="$2";

mkdir -p /usr/cache
mkdir -p /usr/src/work/results
mkdir -p /usr/src/project/results

output_dir="/usr/src/project/"$language"/"$project_name"/results"
output_file="/usr/src/project/"$language"/"$project_name"/results/test_results.txt"

cp -R /usr/src/project/"$language"/"$project_name"/* /usr/src/work

cd /usr/src/work || exit 1

cd ./src
javac "$project_name".java

# Check if compilation was successful
if [ $? -eq 0 ]; then
  echo "Compilation successful. Running the program..."

  # Run Java program
  java "$project_name"

else
  echo "Compilation failed. Please fix the errors."
fi

# Compile the JUnit test file with dependencies in the classpath
javac -cp .:junit-jupiter-api-5.10.1.jar:junit-jupiter-engine-5.10.1.jar:junit-platform-console-standalone-1.9.3.jar:apiguardian-api-1.1.2.jar "$project_name"Tests.java

# Run the compiled JUnit test (replace YourTestClassName with the actual test class name)
java -cp .:junit-jupiter-api-5.10.1.jar:junit-jupiter-engine-5.10.1.jar:junit-platform-console-standalone-1.9.3.jar:apiguardian-api-1.1.2.jar org.junit.platform.console.ConsoleLauncher --select-class "$project_name"Tests > $output_file 2>&1 && echo "Tests passed successfully!"

cp -r /usr/src/work/results/* /usr/src/project/results
