import { promisify } from 'util';
import { exec } from 'child_process';
import { readFile,mkdtemp} from 'fs/promises';
import * as fs from 'fs';
const ncp = require('ncp').ncp;

export async function runCSharp(exerciseName: string, templateFilePath:string,code:string, fileName: string): Promise<any> {
  const solutionDir = await createTempDirAndCopyTemplate(exerciseName, templateFilePath);
  await replaceCode(code, solutionDir,fileName);

  await runCommands(`/usr/src/app/${solutionDir}`, `dotnet restore`);
  await runCommands(`/usr/src/app/${solutionDir}`, `dotnet test -l:trx;LogFileName=TestOutput.xml`);

  const result = await readFile(`/usr/src/app/${solutionDir}/results/testresults.json`, 'utf-8');
  return JSON.parse(result);
}

async function replaceCode(code: string, filePath: string, fileName:string): Promise<void> {
  const templateFilePath = `/usr/src/app/${filePath}/src/${fileName}`;

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
  } catch (error: any) {
    console.error(error.message);
  }
}


async function createTempDirAndCopyTemplate(exerciseName: string, templateFilePath: string): Promise<string> {
  let solutionDir = await mkdtemp(exerciseName);

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