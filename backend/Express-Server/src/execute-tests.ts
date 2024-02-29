import express, { Request, Response } from 'express';
import * as path from 'path';
import { promisify } from 'util';
import { exec } from 'child_process';
import { readdir, readFile,mkdtemp,cp } from 'fs/promises';
import { resolve } from 'path';
import bodyParser from 'body-parser';
import cors from 'cors';
import e from 'express';
import { copyFile } from 'fs';
const ncp = require('ncp').ncp;

export async function runTs(exerciseId: string, templateFilePath:string): Promise<any> {
    const solutionDir = await mkdtemp(exerciseId);
    console.log(solutionDir);
    ncp(templateFilePath, solutionDir, { clobber: false }, function(err: any) {
    if (err) {
      return console.error(err);
    }
    console.log('Copy successful!');
  });
    let path = `/usr/src/app/${solutionDir}`;
    let compile = `npm install`;
    const { stdout, stderr } = await promisify(exec)(compile, { cwd: path });

    path = `/usr/src/app/${solutionDir}/`;
    compile = `npx tsc`;
    const { stdout: secondStdout, stderr: secondStderr } = await promisify(exec)(compile, { cwd: path });
    console.log('secondStdout');

    path = `/usr/src/app/${solutionDir}/`;
    compile = `npm test -- --reporter json --reporter-options output=/usr/src/app/${solutionDir}/results/testresults.json`;
    const { stdout: thirdStdout, stderr: thirdStderr } = await promisify(exec)(compile, { cwd: path });
    console.log('thirdStdout');

    const result = await readFile(`/usr/src/app/${solutionDir}/results/testresults.json`, 'utf-8');

    return result;
  }