import { promisify } from 'util';
import { exec } from 'child_process';
import { readFile,mkdtemp} from 'fs/promises';
import { Snippet } from './model/snippet';
import { Snippets } from './model/snippets';
import * as fs from 'fs';
const ncp = require('ncp').ncp;

export async function runTs(exerciseName: string, templateFilePath:string,snippets:Snippets): Promise<any> {
  const solutionDir = await createTempDirAndCopyTemplate(exerciseName, templateFilePath);
  console.log("2");
  await replaceCode(snippets, solutionDir);
  console.log("3");

  await runCommands(`/usr/src/app/${solutionDir}`, `npm install`);
  await runCommands(`/usr/src/app/${solutionDir}`, `npx tsc`);
  await runCommands(`/usr/src/app/${solutionDir}`, `npm test -- --reporter json --reporter-options output=/usr/src/app/${solutionDir}/results/testresults.json`);

  const result = await readFile(`/usr/src/app/${solutionDir}/results/testresults.json`, 'utf-8');
  return JSON.parse(result);
}

async function replaceCode(snippets: Snippets, filePath: string): Promise<void> {
  const codeDictionary: { [key: string]: string } = {};
  let files: string[] = [];

  snippets.ArrayOfSnippets.forEach((snippet) => {
    if (!codeDictionary[snippet.FileName]) {
      codeDictionary[snippet.FileName] = snippet.Code;
      files.push(snippet.FileName);
    } else {
      // If the filename already exists, concatenate the code
      codeDictionary[snippet.FileName] += '\n' + snippet.Code;
    }
  });

  return new Promise<void>((resolve, reject) => {
    files.forEach((fileName) => {
    const templateFilePath = `/usr/src/app/${filePath}/src/${fileName}`;

    fs.writeFile(templateFilePath, codeDictionary[fileName], (err: any) => {
      if(err){
        console.log(err);
        reject(err);
      }
      else{
        resolve();
      }
    });})});
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

function reject(err: any) {
  throw new Error('Function not implemented.');
}
