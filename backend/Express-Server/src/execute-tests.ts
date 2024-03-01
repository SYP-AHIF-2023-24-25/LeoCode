import { promisify } from 'util';
import { exec } from 'child_process';
import { readFile,mkdtemp,mkdir } from 'fs/promises';
import * as path from 'path';
import * as fs from 'fs';
const ncp = require('ncp').ncp;

async function replaceCode(code: string, filePath: string): Promise<void> {
  console.log("in replace code function");
  //`/usr/src/app/` + 
  const templateFilePath =`/usr/src/app/` + filePath + `/src/passwordChecker.ts`;
  fs.writeFile(templateFilePath, code, (err: any) => {
    if (err) {
      console.log("Error in replacing code");
      console.error(err);
    } else {
      console.log("Code replaced successfully.");
    }
  });
  console.log("finished replacing code");
}

export async function runTs(exerciseId: string, templateFilePath:string,code:string): Promise<any> {
    let solutionDir = await mkdtemp(exerciseId);
    console.log(solutionDir);
    
    //templateFilePath = `usr/src/app/${templateFilePath}`;
    ncp(templateFilePath, solutionDir, { clobber: false }, function(err: any) {
    if (err) {
      console.log("Error in copying");
      return console.error(err);
    }
    console.log('Copy successful!');
    
    });
    await replaceCode(code, solutionDir);
    let path = `/usr/src/app/${solutionDir}`;
    let compile = `npm install`;
    const { stdout, stderr } = await promisify(exec)(compile, { cwd: path });

    path = `/usr/src/app/${solutionDir}/`;
    compile = `npx tsc`;
    const { stdout: secondStdout, stderr: secondStderr } = await promisify(exec)(compile, { cwd: path });

    path = `/usr/src/app/${solutionDir}/`;
    compile = `npm test -- --reporter json --reporter-options output=/usr/src/app/${solutionDir}/results/testresults.json`;
    const { stdout: thirdStdout, stderr: thirdStderr } = await promisify(exec)(compile, { cwd: path });

    const result = await readFile(`/usr/src/app/${solutionDir}/results/testresults.json`,'utf-8');
    console.log(`===============================`);
    console.log(result);
    console.log(`===============================`);
    const jsonData = JSON.parse(result);
    console.log(jsonData);
    console.log(`===============================`);
    return jsonData;
  }