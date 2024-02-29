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
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const express_1 = __importDefault(require("express"));
const fs = __importStar(require("fs"));
const path = __importStar(require("path"));
const util_1 = require("util");
const child_process_1 = require("child_process");
const promises_1 = require("fs/promises");
const path_1 = require("path");
const body_parser_1 = __importDefault(require("body-parser"));
const cors_1 = __importDefault(require("cors"));
const execute_tests_1 = require("./execute-tests");
const swagger_ui_express_1 = __importDefault(require("swagger-ui-express"));
const swaggerDocument = require('../swagger.json');
const app = (0, express_1.default)();
const port = 3000;
app.use('/swagger', swagger_ui_express_1.default.serve, swagger_ui_express_1.default.setup(swaggerDocument));
app.use((0, cors_1.default)());
app.use(body_parser_1.default.json());
app.get('/', (req, res) => {
    res.send('Hello, Express!');
});
app.get('/helloworld', (req, res) => {
    res.send('Hello, World!');
});
app.get('/runtests', (req, res) => __awaiter(void 0, void 0, void 0, function* () {
    console.log("eintritt");
    replaceCode(req.body.code);
    console.log("replaced code");
    const testresults = yield runtests(res, req.body.language, req.body.programName);
    res.status(200).json(testresults);
}));
app.post('/api/execute/:exerciseId', (req, res) => __awaiter(void 0, void 0, void 0, function* () {
    const exerciseId = req.params.exerciseId;
    const templateFilePath = `/usr/src/app/templates/${exerciseId}`;
    console.log(templateFilePath);
    const result = yield (0, execute_tests_1.runTs)(exerciseId, templateFilePath);
    res.status(200).json(result);
}));
app.listen(port, () => {
    console.log(`Server is running at http://localhost:${port}`);
});
function replaceCode(code) {
    console.log("in replace code function");
    const cwd = process.cwd();
    console.log(cwd);
    const templateFilePath = path.join(cwd, '../languages/Typescript/PasswordChecker/src/passwordChecker.ts');
    let templateCode = fs.readFileSync(templateFilePath, 'utf-8');
    templateCode = code;
    fs.writeFileSync(templateFilePath, templateCode);
    console.log("finished replacing code");
}
function runtests(res, language, ProgramName) {
    return __awaiter(this, void 0, void 0, function* () {
        try {
            const cwd = process.cwd();
            const languagesPath = (0, path_1.resolve)(cwd, '../', 'languages');
            console.log(languagesPath);
            const languagePath = (0, path_1.resolve)(languagesPath, language, ProgramName);
            console.log(languagePath);
            const command = `run --rm -v ${languagesPath}:/usr/src/project -w /usr/src/project gutersprint ${language} ${ProgramName}`;
            const { stdout, stderr } = yield (0, util_1.promisify)(child_process_1.exec)(`docker ${command}`);
            const codeResultsPath = (0, path_1.resolve)(languagePath, 'results');
            const files = yield (0, promises_1.readdir)(codeResultsPath);
            const resultsFile = files.find((file) => file.endsWith('.json'));
            if (resultsFile) {
                const jsonString = yield (0, promises_1.readFile)((0, path_1.resolve)(codeResultsPath, resultsFile), 'utf-8');
                console.log('Received JSON:', jsonString); // Logge die empfangenen Daten
                const jsonDocument = JSON.parse(jsonString);
                // Modifiziere die Antwortdaten, um zirkuläre Referenzen zu vermeiden
                const responseString = ({ data: jsonDocument });
                // Setze die Antwortdaten ohne die Response-Instanz
                //return res.status(200).send(jsonString);
                return responseString;
            }
            else {
                const errorObject = { error: 'No results file found.' };
                //return res.status(400).json(errorObject);
                return "";
            }
        }
        catch (ex) {
            console.error('Error during tests:', ex); // Logge den Fehler für die Diagnose
            const errorObject = { error: `An error occurred: ${ex.message}` };
            //return res.status(500).json(errorObject);
            return "";
        }
    });
}
