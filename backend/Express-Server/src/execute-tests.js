"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.runTs = void 0;
const util_1 = require("util");
const child_process_1 = require("child_process");
const promises_1 = require("fs/promises");
const ncp = require('ncp').ncp;
function runTs(exerciseId, templateFilePath) {
    return __awaiter(this, void 0, void 0, function* () {
        const solutionDir = yield (0, promises_1.mkdtemp)(exerciseId);
        console.log('solutionDir');
        console.log(solutionDir);
        console.log('solutionDir');
        // await promisify(copyFile)(templateFilePath, solutionDir);
        ncp(templateFilePath, solutionDir, { clobber: false }, function (err) {
            if (err) {
                return console.error(err);
            }
            console.log('Copy successful!');
        });
        let path = `/usr/src/app/${solutionDir}`;
        let compile = `npm install`;
        const { stdout, stderr } = yield (0, util_1.promisify)(child_process_1.exec)(compile, { cwd: path });
        path = `/usr/src/app/${solutionDir}/`;
        compile = `npx tsc`;
        const { stdout: secondStdout, stderr: secondStderr } = yield (0, util_1.promisify)(child_process_1.exec)(compile, { cwd: path });
        console.log('secondStdout');
        path = `/usr/src/app/${solutionDir}/`;
        compile = `npm test -- --reporter json --reporter-options output=/usr/src/app/${solutionDir}/results/testresults.json`;
        const { stdout: thirdStdout, stderr: thirdStderr } = yield (0, util_1.promisify)(child_process_1.exec)(compile, { cwd: path });
        console.log('thirdStdout');
        /*const command = `npm run test`;
        const { stdout, stderr } = await promisify(exec)(command, { cwd: solutionDir });*/
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
    });
}
exports.runTs = runTs;
