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
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const express_1 = __importDefault(require("express"));
const body_parser_1 = __importDefault(require("body-parser"));
const cors_1 = __importDefault(require("cors"));
const execute_tests_1 = require("./execute-tests");
const fs_1 = __importDefault(require("fs"));
const swagger_ui_express_1 = __importDefault(require("swagger-ui-express"));
const path_1 = __importDefault(require("path"));
const multer_1 = __importDefault(require("multer"));
const unzipper_1 = __importDefault(require("unzipper")); // Add unzipper import
const execute_tests_2 = require("./execute-tests");
const swaggerDocument = require('../swagger.json');
const app = (0, express_1.default)();
const port = 3000;
app.use('/swagger', swagger_ui_express_1.default.serve, swagger_ui_express_1.default.setup(swaggerDocument));
app.use((0, cors_1.default)());
app.use(body_parser_1.default.json());
app.get('/', (req, res) => {
    res.send('Hello, Express!');
});
app.get('/api/code/:exerciseName', (req, res) => __awaiter(void 0, void 0, void 0, function* () {
    try {
        console.log("before suchen ...");
        const exerciseName = req.params.exerciseName;
        const result = yield searchForTsFile(exerciseName);
        console.log("============================");
        console.log(result);
        console.log("============================");
        res.status(200).json(result);
    }
    catch (err) {
        res.status(400).json(err);
    }
}));
function searchForTsFile(exerciseName) {
    return __awaiter(this, void 0, void 0, function* () {
        const directoryPath = `/usr/src/app/templates/${exerciseName}/src`;
        const files = yield fs_1.default.promises.readdir(directoryPath);
        const tsFiles = files.filter(file => file.endsWith('.ts'));
        if (tsFiles.length > 0) {
            const tsFilePath = path_1.default.join(directoryPath, tsFiles[0]);
            const fileContent = yield fs_1.default.promises.readFile(tsFilePath, 'utf-8');
            return fileContent;
        }
        else {
            throw new Error('No .ts file found in the specified directory');
        }
    });
}
app.post('/api/execute/:exerciseName', (req, res) => __awaiter(void 0, void 0, void 0, function* () {
    try {
        const exerciseName = req.params.exerciseName;
        const fileName = req.body.fileName;
        const code = req.body.code;
        const templateFilePath = `./templates/${exerciseName}`;
        console.log(fileName);
        console.log(code);
        const result = yield (0, execute_tests_1.runTs)(exerciseName, templateFilePath, code, fileName);
        //log success
        res.status(200).json(result);
    }
    catch (err) {
        //log error
        res.status(500).json(err);
    }
}));
const storage = multer_1.default.diskStorage({
    destination: function (req, file, cb) {
        cb(null, './templates'); // Save uploaded files to the "uploads" directory
    },
    filename: function (req, file, cb) {
        cb(null, file.originalname);
    }
});
const upload = (0, multer_1.default)({ storage: storage });
// Upload route with zip file extraction
app.post('/uploadFullTemplate', upload.single('file'), (req, res) => __awaiter(void 0, void 0, void 0, function* () {
    // Check if a file was uploaded
    try {
        if (!req.file) {
            return res.status(400).json({ error: 'No file uploaded' });
        }
        // Extract the uploaded zip file
        const zipFilePath = path_1.default.join(__dirname, '../templates', req.file.filename);
        yield unZip(zipFilePath);
        console.log("after unzipping ...");
        const fileNameSplitted = req.file.filename.split('.');
        console.log(fileNameSplitted[0]);
        let result = "...";
        console.log(req.body.content);
        if (req.body.content === "full") {
            result = yield (0, execute_tests_2.runTemplate)(`templates/${fileNameSplitted[0]}`);
            console.log(result);
        }
        else {
            result = "empty template uploaded";
        }
        console.log("before the end ...");
        // Remove the unzipped directory
        if (req.body.content === "full") {
            const unzippedDirPath = path_1.default.join(__dirname, '../templates', fileNameSplitted[0]);
            yield deleteFolderRecursive(unzippedDirPath);
        }
        fs_1.default.unlinkSync(zipFilePath);
        //log success
        res.status(200).json(result);
    }
    catch (err) {
        //log error
        res.status(500).json(err);
    }
}));
function deleteFolderRecursive(path) {
    return __awaiter(this, void 0, void 0, function* () {
        if (fs_1.default.existsSync(path)) {
            fs_1.default.readdirSync(path).forEach((file, index) => {
                const curPath = path + "/" + file;
                if (fs_1.default.lstatSync(curPath).isDirectory()) { // recurse
                    deleteFolderRecursive(curPath);
                }
                else { // delete file
                    fs_1.default.unlinkSync(curPath);
                }
            });
            fs_1.default.rmdirSync(path);
        }
    });
}
function unZip(zipFilePath) {
    return __awaiter(this, void 0, void 0, function* () {
        return new Promise((resolve, reject) => {
            fs_1.default.createReadStream(zipFilePath)
                .pipe(unzipper_1.default.Extract({ path: `/usr/src/app/templates` }))
                .on('close', () => {
                console.log('Zip file extracted successfully');
                resolve();
            })
                .on('error', (err) => {
                console.error('Error extracting zip file:', err);
                reject(err);
            });
        });
    });
}
app.listen(port, () => {
    console.log(`Server is running at http://localhost:${port}`);
});
