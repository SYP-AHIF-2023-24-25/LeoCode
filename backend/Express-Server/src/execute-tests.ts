import express, { Request, Response } from 'express';
import * as path from 'path';
import { promisify } from 'util';
import { exec } from 'child_process';
import { readdir, readFile,mkdtemp,cp } from 'fs/promises';
import { resolve } from 'path';
import bodyParser from 'body-parser';
import cors from 'cors';
import e from 'express';


export async function runTs(exerciseId: string, templateFilePath:string): Promise<any> {
    const solutionDir = await mkdtemp(exerciseId);
    console.log(solutionDir);
    await cp(templateFilePath, solutionDir);
    const command = `npm run test`;
    const { stdout, stderr } = await promisify(exec)(command, { cwd: solutionDir });
    return "Test successful";
    /*try {
      const cwd: string = process.cwd();
      const languagesPath: string = resolve(cwd, '../', 'languages');
      console.log(languagesPath);
      const languagePath: string = resolve(languagesPath, language, ProgramName);
      console.log(languagePath);
      const command: string = `run --rm -v ${languagesPath}:/usr/src/project -w /usr/src/project gutersprint ${language} ${ProgramName}`;
      const { stdout, stderr } = await promisify(exec)(`docker ${command}`);
  
      const codeResultsPath: string = resolve(languagePath, 'results');
      const files = await readdir(codeResultsPath);
  
      const resultsFile: string | undefined = files.find((file) => file.endsWith('.json'));
  
      if (resultsFile) {
        const jsonString: string = await readFile(resolve(codeResultsPath, resultsFile), 'utf-8');
        console.log('Received JSON:', jsonString); // Logge die empfangenen Daten
  
        const jsonDocument = JSON.parse(jsonString);
  
        // Modifiziere die Antwortdaten, um zirkuläre Referenzen zu vermeiden
        const responseString = ({ data: jsonDocument });
  
        // Setze die Antwortdaten ohne die Response-Instanz
        //return res.status(200).send(jsonString);
        return responseString;
      } else {
        const errorObject = { error: 'No results file found.' };
        //return res.status(400).json(errorObject);
        return "";
      }
    } catch (ex: any) {
      console.error('Error during tests:', ex); // Logge den Fehler für die Diagnose
      const errorObject = { error: `An error occurred: ${ex.message}` };
      //return res.status(500).json(errorObject);
      return "";
    }*/
  }