import express, { Request, Response } from 'express';
import * as fs from 'fs';
import * as path from 'path';
import { promisify } from 'util';
import { exec } from 'child_process';
import { readdir, readFile } from 'fs/promises';
import { resolve } from 'path';
import bodyParser from 'body-parser';
import cors from 'cors';

import swaggerUi from 'swagger-ui-express';

const swaggerDocument = require('../swagger.json');

const app = express();
const port = 3000;

app.use('/swagger', swaggerUi.serve, swaggerUi.setup(swaggerDocument));
app.use(cors());

app.use(bodyParser.json());

app.get('/', (req: Request, res: Response) => {
  res.send('Hello, Express!');
});

app.post('/runtests', async (req: Request, res: Response) => {
  console.log("eintritt");
  replaceCode(req.body.code);
  console.log("replaced code");
  const testresults = await runtests(res, req.body.language, req.body.programName);

  res.status(200).json(testresults);
});

app.listen(port, () => {
  console.log(`Server is running at http://localhost:${port}`);
});

function replaceCode(code: string): void {
  console.log("in replace code function");
  const cwd = process.cwd();
  console.log(cwd);
  const templateFilePath = path.join(cwd, '../languages/Typescript/PasswordChecker/src/passwordChecker.ts');
  let templateCode = fs.readFileSync(templateFilePath, 'utf-8');
  templateCode = code;
  fs.writeFileSync(templateFilePath, templateCode);
  console.log("finished replacing code");
}

async function runtests(res: Response, language: string, ProgramName: string): Promise<any> {
  try {
    const cwd: string = process.cwd();
    const languagesPath: string = resolve(cwd, '../', 'languages');
    console.log(languagesPath);
    const languagePath: string = resolve(languagesPath, language, ProgramName);
    console.log(languagePath);
    const command: string = `run --rm -v ${languagesPath}:/usr/src/project -w /usr/src/project line18 ${language} ${ProgramName}`;
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
  }
}