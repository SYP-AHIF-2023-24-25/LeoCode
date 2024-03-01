import { promisify } from 'util';
import { exec } from 'child_process';
import { readFile,mkdtemp,mkdir } from 'fs/promises';
import { Console } from 'console';
const ncp = require('ncp').ncp;

export async function replaceCode(code: string,exerciseId: string): Promise<void> {
  const solutionDir = await mkdir(exerciseId);
  /*console.log("in replace code function");
  const cwd = process.cwd();
  console.log(cwd);
  const templateFilePath = path.join(cwd, '../languages/Typescript/PasswordChecker/src/passwordChecker.ts');
  let templateCode = fs.readFileSync(templateFilePath, 'utf-8');
  templateCode = code;
  fs.writeFileSync(templateFilePath, templateCode);
  console.log("finished replacing code");*/
}

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

    const result = await readFile(`/usr/src/app/${solutionDir}/results/testresults.json`,'utf-8');
    console.log(`===============================`);
    console.log(result);
    console.log(`===============================`);
    const jsonData = JSON.parse(result);
    console.log(jsonData);
    console.log(`===============================`);
    return jsonData;
  }