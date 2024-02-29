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
        console.log(solutionDir);
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
        const result = yield (0, promises_1.readFile)(`/usr/src/app/${solutionDir}/results/testresults.json`, 'utf-8');
        return result;
    });
}
exports.runTs = runTs;
