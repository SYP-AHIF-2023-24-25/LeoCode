"use strict";
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    var desc = Object.getOwnPropertyDescriptor(m, k);
    if (!desc || ("get" in desc ? !m.__esModule : desc.writable || desc.configurable)) {
      desc = { enumerable: true, get: function() { return m[k]; } };
    }
    Object.defineProperty(o, k2, desc);
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
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
const fs = __importStar(require("fs"));
const ncp = require('ncp').ncp;
function runTs(exerciseName, templateFilePath, code, fileName) {
    return __awaiter(this, void 0, void 0, function* () {
        const solutionDir = yield createTempDirAndCopyTemplate(exerciseName, templateFilePath);
        yield replaceCode(code, solutionDir, fileName);
        console.log("3");
        yield runCommands(`/usr/src/app/${solutionDir}`, `npm install`);
        yield runCommands(`/usr/src/app/${solutionDir}`, `npx tsc`);
        yield runCommands(`/usr/src/app/${solutionDir}`, `npm test -- --reporter json --reporter-options output=/usr/src/app/${solutionDir}/results/testresults.json`);
        const result = yield (0, promises_1.readFile)(`/usr/src/app/${solutionDir}/results/testresults.json`, 'utf-8');
        return JSON.parse(result);
    });
}
exports.runTs = runTs;
function replaceCode(code, filePath, fileName) {
    return __awaiter(this, void 0, void 0, function* () {
        console.log("happyyyyyyy");
        const templateFilePath = `/usr/src/app/${filePath}/src/${fileName}`;
        return new Promise((resolve, reject) => {
            fs.writeFile(templateFilePath, code, (err) => {
                if (err) {
                    console.error(err);
                    reject(err);
                }
                else {
                    resolve();
                }
            });
        });
    });
}
function runCommands(path, command) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const { stdout, stderr } = yield (0, util_1.promisify)(child_process_1.exec)(command, { cwd: path });
        }
        catch (error) {
            console.error(error.message);
        }
    });
}
function createTempDirAndCopyTemplate(exerciseName, templateFilePath) {
    return __awaiter(this, void 0, void 0, function* () {
        let solutionDir = yield (0, promises_1.mkdtemp)(exerciseName);
        yield new Promise((resolve, reject) => {
            ncp(templateFilePath, solutionDir, { clobber: false }, function (err) {
                if (err) {
                    reject(err);
                }
                else {
                    resolve();
                }
            });
        });
        return solutionDir;
    });
}
