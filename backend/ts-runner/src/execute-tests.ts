import { promisify } from 'util';
import { exec } from 'child_process';
import { readFile,mkdtemp} from 'fs/promises';
import { Snippet } from './model/snippet';
import { Snippets } from './model/snippets';
import * as fs from 'fs';
const ncp = require('ncp').ncp;

export async function runTs(exerciseName: string, templateFilePath:string,code:string,fileName:string): Promise<any> {
  const solutionDir = await createTempDirAndCopyTemplate(exerciseName, templateFilePath);
  await runCommands(`/usr/src/app/${solutionDir}`, `ln -s /usr/src/app/modules/node_modules /usr/src/app/${solutionDir}/node_modules`);
  await replaceCode(code, solutionDir,fileName);
  await runCommands(`/usr/src/app/${solutionDir}`, `npx tsc`);

  await runCommands(`/usr/src/app/${solutionDir}`, `npm test -- --reporter json --reporter-options output=/usr/src/app/${solutionDir}/results/testresults.json`);

  const result = await readFile(`/usr/src/app/${solutionDir}/results/testresults.json`, 'utf-8');
  return JSON.parse(result);
}

export async function runTemplate(path:string): Promise<any> {
  console.log("before testing");
  await runCommands(`/usr/src/app/${path}`, `ln -s /usr/src/app/modules/node_modules /usr/src/app/${path}/node_modules`);
  await runCommands(`/usr/src/app/${path}`, `npx tsc`);
  await runCommands(`/usr/src/app/${path}`, `npm test -- --reporter json --reporter-options output=/usr/src/app/${path}/results/testresults.json`);
  console.log("after testing");
  const result = await readFile(`/usr/src/app/${path}/results/testresults.json`, 'utf-8');
  console.log("after reading file");
  return JSON.parse(result);
}

async function replaceCode(code: string, filePath: string,fileName:string): Promise<void> {
  console.log("happyyyyyyy");
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
