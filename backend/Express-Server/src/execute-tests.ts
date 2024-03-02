import { promisify } from 'util';
import { exec } from 'child_process';
import { readFile,mkdtemp,mkdir } from 'fs/promises';
import * as path from 'path';
import * as fs from 'fs';
const ncp = require('ncp').ncp;

export async function runTs(exerciseId: string, templateFilePath:string,code:string): Promise<any> {
  const solutionDir = await createTempDirAndCopyTemplate(exerciseId, templateFilePath);
  await replaceCode(code, solutionDir);

  await runCommands(`/usr/src/app/${solutionDir}`, `npm install`);
  await runCommands(`/usr/src/app/${solutionDir}`, `npx tsc`);
  await runCommands(`/usr/src/app/${solutionDir}`, `npm test -- --reporter json --reporter-options output=/usr/src/app/${solutionDir}/results/testresults.json`);

  const result = await readFile(`/usr/src/app/${solutionDir}/results/testresults.json`, 'utf-8');
  const jsonData = JSON.parse(result);
  return jsonData;
}

async function replaceCode(code: string, filePath: string): Promise<void> {
  const templateFilePath = `/usr/src/app/${filePath}/src/passwordChecker.ts`;

  return new Promise<void>((resolve, reject) => {
    fs.writeFile(templateFilePath, code, (err: any) => {
      if (err) {
        console.error(err);
        reject(err);
      } else {
        resolve();
      }
    });
  });
}

async function runCommands(path: string, command: string): Promise<void> {
  try {
    const { stdout, stderr } = await promisify(exec)(command, { cwd: path });
    console.log('Alles richtig:');
  } catch (error: any) {
    console.error('Nicht alles richtig:');
    // Handle the error as needed (you can log it or take other actions)
    // For now, we're not rethrowing the error to prevent it from propagating
  }
}


async function createTempDirAndCopyTemplate(exerciseId: string, templateFilePath: string): Promise<string> {
  let solutionDir = await mkdtemp(exerciseId);

  await new Promise<void>((resolve, reject) => {
    ncp(templateFilePath, solutionDir, { clobber: false }, function (err: any) {
      if (err) {
        reject(err);
      } else {
        resolve();
      }
    });
  });

  return solutionDir;
}