// src/app.ts
import express, { Request, Response } from 'express';
import * as fs from 'fs';
import * as path from 'path';
import { promisify } from 'util';
import { exec } from 'child_process';
import { readdir, readFile } from 'fs/promises';
import { resolve } from 'path';
import { Exception } from '@azure/functions';

const app = express();
const port = 3000;

app.get('/', (req: Request, res: Response) => {
  res.send('Hello, Express!');
});

app.post('/runtest', (req: Request, res: Response) => {
  replaceCode(req.body.code);
  const testresults = runtests(res, req.body.language, req.body.ProgramName);

  res.status(200).json(testresults);
});

app.listen(port, () => {
  console.log(`Server is running at http://localhost:${port}`);
});

function replaceCode(code: string): void {
  const cwd = process.cwd();
  const templateFilePath = path.join(cwd, '../languages/Typescript/PasswordChecker/src/passwordChecker.ts');
  let templateCode = fs.readFileSync(templateFilePath, 'utf-8');
  templateCode = code;
  fs.writeFileSync(templateFilePath, templateCode);
}

async function runtests(res:Response, language:string, ProgramName:string): Promise<Response> {
  try {
    const cwd: string = process.cwd();
    const languagesPath: string = resolve(cwd, '..', 'languages');
    const languagePath: string = resolve(languagesPath, language, ProgramName);

    const command: string = `run --rm -v ${languagesPath}:/usr/src/project -w /usr/src/project passwordchecker ${language} ${ProgramName}`;
    const { stdout, stderr } = await promisify(exec)(`docker ${command}`);

    const codeResultsPath: string = resolve(languagePath, 'results');
    const files = await readdir(codeResultsPath);

    const resultsFile: string | undefined = files.find((file) => file.endsWith('.json'));

    if (resultsFile) {
      const jsonString: string = await readFile(resolve(codeResultsPath, resultsFile), 'utf-8');
      const jsonDocument = JSON.parse(jsonString);
      const responseObject = { data: jsonDocument };

      return res.status(200).send(responseObject);
    } else {
      const errorObject = { error: 'No results file found.' };
      return res.status(400).send(errorObject);
    }
  } catch (ex: any) {
    const errorObject = { error: `An error occurred: ${ex.message}` };
    return res.status(500).send(errorObject);
  }
}